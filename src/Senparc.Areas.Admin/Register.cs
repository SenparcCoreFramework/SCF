using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Core.Areas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin
{
    public class Register : IAreaRegister
    {
        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
        {
            builder.AddRazorPagesOptions(options => {
                options.Conventions.AuthorizePage("/Admin");//必须登录
                options.Conventions.AllowAnonymousToPage("/Admin/Login");//允许匿名
                //更多：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-2.2
            });

            return builder;
        }
    }
}
