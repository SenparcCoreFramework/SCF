using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Senparc.Scf.Core.Exntesions
{
    public static class RegisterExtension
    {
        public static void AddScfServices(this IServiceCollection services, IConfiguration configuration, CompatibilityVersion compatibilityVersion)
        {
            //如果运行在IIS中，需要添加IIS配置
            //https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.1&tabs=aspnetcore2x#supported-operating-systems
            //services.Configure<IISOptions>(options =>
            //{
            //    options.ForwardClientCertificate = false;
            //});


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc(options =>
            {
                //options.Filters.Add<HttpGlobalExceptionFilter>();
            })
            .SetCompatibilityVersion(compatibilityVersion)
            .AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;//支持 Area
            })
            .AddScfAreas()//注册所有 Scf 的 Area 模块（必须）
            .AddXmlSerializerFormatters()
            .AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            })
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1&tabs=aspnetcore2x
            //.AddSessionStateTempDataProvider()
            //忽略JSON序列化过程中的循环引用：https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
            ;


            //支持 Session
            services.AddSession();
            //解决中文进行编码问题
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //使用内存缓存
            services.AddMemoryCache();

            //注册 SignalR
            services.AddSignalR();
            //注册 Lazy<T>
            services.AddTransient(typeof(Lazy<>));

            services.Configure<SenparcCoreSetting>(configuration.GetSection("SenparcCoreSetting"))
                .AddSenparcEntitiesDI(); //SQL Server设置


            //注册User
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAnonymous", policy =>
                {
                    policy.RequireClaim("UserMember");
                });
            });
        }
    }
}
