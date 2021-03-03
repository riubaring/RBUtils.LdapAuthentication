using System;
using System.Collections.Generic;
using System.Text;

namespace RBUtils.LdapAuthentication.Core.Models
{
    /// <summary>
    /// A model for cookies configuration for <see cref="CookieAuthenticationOptions"/>.
    /// </summary>
    public class CookiesConfig
    {
        /// <summary>
        /// <see cref="CookieAuthentication.Cookie.Name"/>
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// <see cref="CookieAuthenticationOptions.LoginPath"/>.
        /// </summary>
        public string LoginPath { get; set; }

        /// <summary>
        /// <see cref="CookieAuthenticationOptions.LogoutPath"/>
        /// </summary>
        public string LogoutPath { get; set; }

        /// <summary>
        /// <see cref="CookieAuthenticationOptions.AccessDeniedPath"/>
        /// </summary>
        public string AccessDeniedPath { get; set; }

        /// <summary>
        /// <see cref="CookieAuthenticationOptions.ReturnUrlParameter"/>
        /// </summary>
        public string ReturnUrlParameter { get; set; }
    }
}
