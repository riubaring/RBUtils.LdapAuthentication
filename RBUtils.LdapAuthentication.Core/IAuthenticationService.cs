using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RBUtils.LdapAuthentication.Core
{
    public interface IAuthenticationService
    {
        HttpContext Context { get; set; }

        Task<LogInResult> LogInAsync(string userName, string plainPassword, bool isPersistent, List<Claim> userClaims = null);
        Task LogOutAsync();
    }
}
