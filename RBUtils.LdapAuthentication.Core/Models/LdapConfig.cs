using System;
using System.Collections.Generic;
using System.Text;

namespace RBUtils.LdapAuthentication.Core.Models
{
    /// <summary>
    /// A model for ldap configuration.
    /// </summary>
    public class LdapConfig
    {
        /// <summary>
        /// Ldap path.
        /// Example: LDAP://CN=Users,DC=im,DC=baring,DC=org"
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The domain name [For future use]
        /// Example: "im.baring.org"
        /// </summary>
        public string UserDomainName { get; set; }

        /// <summary>
        /// An error message to inform that login attempts failed
        /// Example: "We didn't recognize your user name and password. Please try again."
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
