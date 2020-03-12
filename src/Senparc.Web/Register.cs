﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Senparc.CO2NET.Trace;
using Senparc.Respository;
using Senparc.Scf.Core;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.AssembleScan;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using Senparc.Scf.SMS;
using Senparc.Scf.XscfBase;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Senparc.Web
{
    /// <summary>
    /// 全局注册
    /// </summary>
    public static class Register
    {
        public static void AddScfServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env, CompatibilityVersion compatibilityVersion)
        {
            //如果运行在IIS中，需要添加IIS配置
            //https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.1&tabs=aspnetcore2x#supported-operating-systems
            //services.Configure<IISOptions>(options =>
            //{
            //    options.ForwardClientCertificate = false;
            //});

            //提供网站根目录
            if (env.ContentRootPath != null)
            {
                SiteConfig.ApplicationPath = env.ContentRootPath;
                SiteConfig.WebRootPath = env.WebRootPath;
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddMvc(options =>
            //{
            //    //options.Filters.Add<HttpGlobalExceptionFilter>();
            //})
            //.SetCompatibilityVersion(compatibilityVersion)
            //.AddRazorPagesOptions(options =>
            //{
            //    //options.AllowAreas = true;//支持 Area
            //    //options.AllowMappingHeadRequestsToGetHandler = false;//https://www.learnrazorpages.com/razor-pages/handler-methods
            //})


            var builder = services.AddRazorPages(opt =>
                {
                    //opt.RootDirectory = "/";
                })
              .AddScfAreas(env)//注册所有 Scf 的 Area 模块（必须）
              .AddXmlSerializerFormatters()
              .AddJsonOptions(options =>
              {
                  //忽略循环引用
                  //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  //不使用驼峰样式的key
                  //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                  //设置时间格式
                  //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
              })
              //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1&tabs=aspnetcore2x
              //.AddSessionStateTempDataProvider()
              //忽略JSON序列化过程中的循环引用：https://stackoverflow.com/questions/7397207/json-net-error-self-referencing-loop-detected-for-type
              .AddRazorPagesOptions(options =>
              {
                  //自动注册  防止跨站请求伪造（XSRF/CSRF）攻击
                  options.Conventions.Add(new Core.Conventions.AutoValidateAntiForgeryTokenModelConvention());
              });
            ;

#if DEBUG
            //Razor启用运行时编译，多个项目不需要手动编译。
            if (env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation(options =>
                {
                    //自动索引所有需要使用 RazorRuntimeCompilation 的模块
                    foreach (var razorRegister in Senparc.Scf.XscfBase.Register.RegisterList.Where(z => z is IXscfRazorRuntimeCompilation))
                    {
                        try
                        {
                            var libraryPath = ((IXscfRazorRuntimeCompilation)razorRegister).LibraryPath;
                            options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                        }
                        catch (Exception ex)
                        {
                            SenparcTrace.BaseExceptionLog(ex);
                        }
                    }
                });
            }

#endif

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

            services.Configure<SenparcCoreSetting>(configuration.GetSection("SenparcCoreSetting"));
            services.Configure<SenparcSmsSetting>(configuration.GetSection("SenparcSmsSetting"));

            //自动依赖注入扫描
            services.ScanAssamblesForAutoDI();
            //已经添加完所有程序集自动扫描的委托，立即执行扫描（必须）
            AssembleScanHelper.RunScan();
            //services.AddSingleton<Core.Cache.RedisProvider.IRedisProvider, Core.Cache.RedisProvider.StackExchangeRedisProvider>();

            //注册 User 登录策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAnonymous", policy =>
                {
                    policy.RequireClaim("UserMember");
                });
            });
            services.AddHttpContextAccessor();

            //Attributes
            services.AddScoped(typeof(Senparc.Scf.AreaBase.Admin.Filters.AuthenticationResultFilterAttribute));
            services.AddScoped(typeof(Senparc.Scf.AreaBase.Admin.Filters.AuthenticationAsyncPageFilterAttribute));

            //Repository
            services.AddScoped(typeof(Senparc.Scf.Repository.IRepositoryBase<>), typeof(Senparc.Scf.Repository.RepositoryBase<>));
            services.AddScoped(typeof(ServiceBase<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddScoped(typeof(ISysButtonRespository), typeof(SysButtonRespository));
            //Other
            services.AddScoped(typeof(Scf.Core.WorkContext.Provider.IAdminWorkContextProvider), typeof(Scf.Core.WorkContext.Provider.AdminWorkContextProvider));
            services.AddTransient<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor, Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor>();

            //激活 Xscf 扩展引擎
            services.StartEngine();
        }

        public static void UseScf(this IApplicationBuilder app, IOptions<SenparcCoreSetting> senparcCoreSetting)
        {
            Senparc.Scf.Core.Config.SiteConfig.SenparcCoreSetting = senparcCoreSetting.Value;
            Senparc.Scf.XscfBase.Register.UseScfModules(app);
        }
    }
}
