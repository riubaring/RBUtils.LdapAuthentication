using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RBUtils.LdapAuthentication.Core.Models;

namespace RBUtils.LdapAuthentication.Core
{
    public static class AuthenticationServiceExtensions
    {
        public static void AddLdapAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Load Ldap configuration. Use default, if null
            services.Configure<LdapConfig>(configuration.GetSection("Ldap"));

            // Load Cookie configuration. Use default, if null
            var cookieConfig = configuration.GetSection("Cookies").Get<CookiesConfig>();
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                 {
                     options.Cookie.Name = cookieConfig.CookieName;
                     options.LoginPath = cookieConfig.LoginPath;
                     options.LogoutPath = cookieConfig.LogoutPath;
                     options.AccessDeniedPath = cookieConfig.AccessDeniedPath;
                     options.ReturnUrlParameter = cookieConfig.ReturnUrlParameter;
                 });

            // Make sure IHttpContextAccessor is added
            services.AddHttpContextAccessor();

            // Add the AuthenticationService
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
