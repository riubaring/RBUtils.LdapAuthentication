using System;

namespace RBUtils.LdapAuthentication.Core
{
    public class LogInResult
    {
        private static readonly LogInResult _success = new LogInResult { Succeeded = true };
        private static readonly LogInResult _failed = new LogInResult();
        private static readonly LogInResult _notAllowed = new LogInResult { IsNotAllowed = true };

        public bool Succeeded { get; protected set; }
        public bool IsNotAllowed { get; protected set; }

        public static LogInResult Success => _success;
        public static LogInResult Failed => _failed;
        public static LogInResult NotAllowed => _notAllowed;

        public override string ToString()
        {
            return IsNotAllowed ? "NotAllowed" :
                Succeeded ? "Succeeded" : "Failed";
        }
    }
}
