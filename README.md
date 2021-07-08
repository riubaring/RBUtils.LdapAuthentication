# RBUtils.LdapAuthentication.Core
An LDAP Authentication Service to help to validate user's credentials against an Active Directory.

## How to Use
### Startup.cs
Make sure to add the following syntax in **ConfigureService** method
```
services.AddLdapAuthentication(Configuration)
```
### appsettings.json
Make sure the following sections exist in **appsettings.json**
```
"Ldap": {
    "Path": "LDAP://CN=Users,DC=im,DC=baring,DC=org",
    "UserDomainName": "im.baring.org",
    "ErrorMessage": "We didn't recognize your user name and password. Please try again"
  },
  "Cookies": {
    "CookieName": "MyLdapAuthentication",
    "LoginPath": "/account/login",
    "LogoutPath": "/account/logout",
    "AccessDeniedPath": "/account/accessDenied",
    "ReturnUrlParameter": "returnUrl"
  }
 ```
