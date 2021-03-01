using System;
using System.Collections.Generic;
using System.Text;

namespace RBUtils.LdapAuthentication.Core.Models
{
    public class LdapConfig
    {
        public string Path { get; set; }
        public string UserDomainName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
