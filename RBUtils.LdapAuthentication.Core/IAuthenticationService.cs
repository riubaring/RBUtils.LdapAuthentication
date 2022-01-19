using System.Security.Claims;
using System.Threading.Tasks;

namespace RBUtils.LdapAuthentication.Core
{
    /// <summary>
    /// Provides an abstraction for <see cref="LdapAuthentication"
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// The <see cref="HttpContext"/> used.
        /// </summary>
        HttpContext Context { get; set; }

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
        Task<LogInResult> LogInAsync(string userName, string plainPassword, bool isPersistent, List<Claim> userClaims = null);

        /// <summary>
        /// Logs the current user out of the application.
        /// </summary>
        /// <returns></returns>
        Task LogOutAsync();
    }
}
