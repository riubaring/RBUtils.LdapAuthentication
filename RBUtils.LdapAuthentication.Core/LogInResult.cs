namespace RBUtils.LdapAuthentication.Core
{
    /// <summary>
    /// Represents the result of a login operation.
    /// </summary>
    public class LogInResult
    {
        private static readonly LogInResult _success = new() { Succeeded = true };
        private static readonly LogInResult _failed = new();
        private static readonly LogInResult _notAllowed = new() { IsNotAllowed = true };

        /// <summary>
        /// Returns a flag indicating whether or not login was successful.
        /// </summary>
        /// <value>If login was successful, return True. Otherwise, False.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Returns a flag indicating whether a user trying to login is not allowed to login.
        /// </summary>
        /// <value>If the user trying to login is not allowed to login, return True. Otherwise, False.</value>
        public bool IsNotAllowed { get; protected set; }

        /// <summary>
        /// Returns a <see cref="LogInResult"/> that represents a successful login.
        /// </summary>
        /// <returns>A <see cref="LogInResult"/> that represents a successful login.</returns>
        public static LogInResult Success => _success;

        /// <summary>
        /// Returns a <see cref="LogInResult"/> that represents a failed login.
        /// </summary>
        /// <returns>A <see cref="LogInResult"/> that represents a failed login.</returns>
        public static LogInResult Failed => _failed;

        /// <summary>
        /// Returns a <see cref="LogInResult"/> that represents a login attempt that failed because 
        /// the user is not allowed to login.
        /// </summary>
        /// <returns>A <see cref="LogInResult"/> that represents login attempt that failed due to the
        /// user is not allowed to login.</returns>
        public static LogInResult NotAllowed => _notAllowed;

        /// <summary>
        /// Converts the value of the current <see cref="LogInResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="LogInResult"/> object.</returns>
        public override string ToString()
        {
            return IsNotAllowed ? "NotAllowed" :
                Succeeded ? "Succeeded" : "Failed";
        }
    }
}
