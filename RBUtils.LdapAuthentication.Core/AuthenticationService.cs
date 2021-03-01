using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RBUtils.LdapAuthentication.Core.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RBUtils.LdapAuthentication.Core
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly LdapConfig _ldapConfig;
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _httpContext;

        public HttpContext HttpContext
        {
            get
            {
                var ctx = _httpContext ?? _contextAccessor?.HttpContext;
                if(ctx == null)
                {
                    throw new InvalidOperationException("HttpContext is required.");
                }
                return ctx;
            }
            set
            {
                _httpContext = value;
            }
        }

        public AuthenticationService(IHttpContextAccessor contextAccessor, IOptions<LdapConfig> ldapAccessor)
        {
            _contextAccessor = contextAccessor;
            _ldapConfig = ldapAccessor.Value;
        }

        public async Task<LogInResult> LogInAsync(string userName, string plainPassword, bool isPersistent, List<Claim> userClaims = null)
        {
            try
            {
                using(var principalContext = new PrincipalContext(ContextType.Domain, _ldapConfig.UserDomainName))
                {
                    if(principalContext.ValidateCredentials(userName, plainPassword))
                    {
                        UserPrincipal authenticatedUser = UserPrincipal.FindByIdentity(principalContext, userName);

                        if(authenticatedUser != null)
                        {
                            // Please refer https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-5.0

                            var authenticatedUserClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, authenticatedUser.SamAccountName), // Other options: UserPrincipalName, EmployeeId
                                new Claim("DisplayName", authenticatedUser.DisplayName),
                                new Claim("InitialName", authenticatedUser.GivenName.Substring(0, 1) + authenticatedUser.Surname.Substring(0, 1)),
                                new Claim(ClaimTypes.Email, authenticatedUser.EmailAddress)
                            };

                            var claimsIdentity = new ClaimsIdentity(
                                authenticatedUserClaims.Concat(userClaims), CookieAuthenticationDefaults.AuthenticationScheme);
                            // Roles are to be added to claimsIdentity before sigining in to HttpContext

                            var authProperties = GetAuthenticationProperties(isPersistent);
                            await _httpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme, 
                                new ClaimsPrincipal(claimsIdentity), 
                                authProperties);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return LogInResult.Failed;
        }

        public async Task LogOutAsync()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private AuthenticationProperties GetAuthenticationProperties(bool isPersistent = false)
        {
            return new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = isPersistent,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };
        }

        private string GetGroup(string value)
        {
            Match match = Regex.Match(value, "^CN=([^,]*)");
            if (!match.Success)
            {
                return null;
            }

            return match.Groups[1].Value;
        }
    }
}
