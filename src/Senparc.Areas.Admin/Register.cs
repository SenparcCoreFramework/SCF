using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Areas.Admin.Filters;
using Senparc.CO2NET.Trace;
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
            //鉴权配置
            //添加基于Cookie的权限验证：https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.1&tabs=aspnetcore2x
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

            builder.AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizePage("/", "AdminOnly");//必须登录
                options.Conventions.AllowAnonymousToPage("/Login");//允许匿名
                //更多：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/razor-pages-authorization?view=aspnetcore-2.2
            });

            SenparcTrace.SendCustomLog("系统启动", "完成 Area:Admin 注册");
           
            return builder;
        }
    }
}
