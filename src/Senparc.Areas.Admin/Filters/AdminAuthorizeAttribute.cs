using Microsoft.AspNetCore.Authorization;
using Senparc.Scf.Core.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin.Filters
{
    /// <summary>
    /// 当前 Area 授权处理特性
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        //AuthorizeAttribute 可以和 MVC 通用：https://docs.microsoft.com/en-us/aspnet/core/razor-pages/filter?view=aspnetcore-2.2
        public static string AuthenticationScheme => SiteConfig.ScfAdminAuthorizeScheme;

        public AdminAuthorizeAttribute()
        {
            base.AuthenticationSchemes = AuthenticationScheme;
        }
        public AdminAuthorizeAttribute(string policy) : this()
        {
            this.Policy = policy;
        }
    }
}
