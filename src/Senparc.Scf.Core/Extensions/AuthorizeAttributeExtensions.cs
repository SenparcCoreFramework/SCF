using Microsoft.AspNetCore.Authorization;

namespace Senparc.Scf.Core.Extensions
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public const string AuthenticationScheme = "SenparcAdminAuthorizeScheme";
        public AdminAuthorizeAttribute()
        {
            base.AuthenticationSchemes = AuthenticationScheme;
        }
        public AdminAuthorizeAttribute(string policy) : this()
        {
            this.Policy = policy;
        }
    }

    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public const string AuthenticationScheme = "SenparcUserAuthorizeScheme";
        public UserAuthorizeAttribute(string policy) : this()
        {
            this.Policy = policy;
        }
        public UserAuthorizeAttribute()
        {
            base.AuthenticationSchemes = AuthenticationScheme;
        }
    }
}
