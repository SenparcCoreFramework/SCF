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
using Senparc.CO2NET;

namespace Senparc.Scf.Core
{
    /// <summary>
    /// SCF 的注册程序
    /// </summary>
    public static class Register
    {
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
