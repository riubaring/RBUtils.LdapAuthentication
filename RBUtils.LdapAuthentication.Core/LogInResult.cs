using System;

namespace RBUtils.LdapAuthentication.Core
{
    public class LogInResult
    {
        public static readonly LogInResult _success = new LogInResult { Succeeded = true };
        public static readonly LogInResult _failed = new LogInResult();
        public static readonly LogInResult _notAllowed = new LogInResult { IsNotAllowed = true };

        public bool Succeeded { get; protected set; }
        public bool IsNotAllowed { get; protected set; }

        public static LogInResult Success => _success;
        public static LogInResult Failed => _failed;
        public static LogInResult NotAllowed => _notAllowed;
    }
}
