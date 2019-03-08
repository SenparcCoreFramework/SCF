using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.AssembleScan;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AutoMapper;
using Senparc.CO2NET.RegisterServices;

namespace Senparc.Scf.Core
{
    /// <summary>
    /// SCF 的注册程序
    /// </summary>
    public static class Register
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
                //options.AllowMappingHeadRequestsToGetHandler = false;//https://www.learnrazorpages.com/razor-pages/handler-methods
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

            //services.AddSenparcGlobalServices(configuration);//Senparc.CO2NET 全局注册    //已经在startup.cs中注册

            //支持 AutoMapper
            services.AddAutoMapper();

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


            //自动依赖注入扫描
            services.ScanAssamblesForAutoDI();
            //已经添加完所有程序集自动扫描的委托，立即执行扫描（必须）
            AssembleScanHelper.RunScan();


            //注册 User 登录策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserAnonymous", policy =>
                {
                    policy.RequireClaim("UserMember");
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddScoped(typeof(ISqlClientFinanceData), typeof(SqlClientFinanceData));
        }

        /// <summary>
        /// 扫描自动依赖注入的接口
        /// </summary>
        public static IServiceCollection ScanAssamblesForAutoDI(this IServiceCollection services)
        {
            //遍历所有程序集进行注册
            AssembleScanHelper.AddAssembleScanItem(assembly =>
            {
                var areaRegisterTypes = assembly.GetTypes() //.GetExportedTypes()
                               .Where(z => !z.IsAbstract && !z.IsInterface && z.GetInterface("IAutoDI") != null)
                               .ToArray();

                DILifecycleType dILifecycleType = DILifecycleType.Scoped;

                foreach (var registerType in areaRegisterTypes)
                {
                    try
                    {
                        //判断特性标签
                        var attrs = System.Attribute.GetCustomAttributes(registerType, false).Where(z => z is AutoDITypeAttribute);
                        if (attrs.Count() > 0)
                        {
                            var attr = attrs.First() as AutoDITypeAttribute;
                            dILifecycleType = attr.DILifecycleType;//使用指定的方式
                        }

                        //针对不同的类型进行不同生命周期的 DI 设置
                        switch (dILifecycleType)
                        {
                            case DILifecycleType.Scoped:
                                services.AddScoped(registerType);
                                break;
                            case DILifecycleType.Singleton:
                                services.AddSingleton(registerType);
                                break;
                            case DILifecycleType.Transient:
                                services.AddTransient(registerType);
                                break;
                            default:
                                throw new NotImplementedException($"未处理此 DILifecycleType 类型：{dILifecycleType.ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        SenparcTrace.BaseExceptionLog(ex);
                    }
                }
            }, false);

            return services;
        }
    }
}
