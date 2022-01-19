global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Options;
global using RBUtils.LdapAuthentication.Core.Models;
global using System;
global using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RBUtils.LdapAuthentication.Core
{
    /// <summary>
    /// Provides the APIs for user login opeations.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly LdapConfig _ldapConfig;
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context;

        /// <summary>
        /// The <see cref="HttpContext"/> used.
        /// </summary>
        public HttpContext Context
        {
            get
            {
                var context = _context ?? _contextAccessor?.HttpContext;
                if(context == null)
                {
                    throw new InvalidOperationException("HttpContext is required.");
                }
                return context;
            }
            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="AuthenticationService"/>.
        /// </summary>
        /// <param name="contextAccessor">The accessor used to access the <see cref="HttpContext"/>.</param>
        /// <param name="ldapAccessor">The accessor used to access the <see cref="LdapConfig"/>.</param>
        public AuthenticationService(IHttpContextAccessor contextAccessor, IOptions<LdapConfig> ldapAccessor)
        {
            _contextAccessor = contextAccessor;
            _ldapConfig = ldapAccessor.Value;
        }

        /// <summary>
        /// Login attempt using the specified <paramref name="userName"/> and <paramref name="plainPassword"/>
        /// combination in an asynchronous operations.
        /// </summary>
        /// <param name="userName">The user name to login.</param>
        /// <param name="plainPassword">The password in plain text to login with.</param>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="userClaims">A <see cref="Claim"/> collection of claim type and value.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="LogInResult"/>
        /// for the sign-in attempt.</returns>
        [SupportedOSPlatform("windows")]
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
                                userClaims == null ? authenticatedUserClaims : authenticatedUserClaims.Concat(userClaims), 
                                this.GetType().Name);

                            var authProperties = GetAuthenticationProperties(isPersistent);
                            await Context.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme, 
                                new ClaimsPrincipal(claimsIdentity), 
                                authProperties);

                            return LogInResult.Success;
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }

            return LogInResult.Failed;
        }

        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        /// <returns></returns>
        public async Task LogOutAsync()
        {
            await Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AuthenticationProperties"/>.
        /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-5.0
        /// </summary>
        /// <param name="isPersistent">Flag indicating whether the sign-in cookie should persist after the browser is closed. Default, False.</param>
        /// <returns>A new instance of <see cref="AuthenticationProperties"/> with the specified <paramref name="isPersistent"/>.</returns>
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
