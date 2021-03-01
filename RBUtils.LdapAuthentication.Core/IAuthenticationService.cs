using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RBUtils.LdapAuthentication.Core
{
    public interface IAuthenticationService
    {
        HttpContext HttpContext { get; set; }

        Task<LogInResult> LogInAsync(string userName, string plainPassword, bool isPersistent);
        Task LogOut();
    }
}
