using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin
{
    public class Register : IAreaRegister
    {
        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
        {
            builder.AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/Admin", "AdminOnly");//必须登录
                options.Conventions.AllowAnonymousToPage("/Admin/Login");//允许匿名
                //更多：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-2.2
            });

            //鉴权配置

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(AdminAuthorizeAttribute.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Admin/Forbidden/";
                    options.LoginPath = "/Admin/Login/";
                    options.Cookie.HttpOnly = false;
                });
            builder.Services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("AdminOnly", policy =>
                    {
                        policy.RequireClaim("AdminMember");
                    });
                });
            return builder;
        }
    }
}
