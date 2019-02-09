using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Scf.Core.Areas
{
    /// <summary>
    /// 对所有扩展 Area 进行注册
    /// </summary>
    public static class AreaRegister
    {
        public static bool RegisterAreasFinished { get; set; }

        public static IMvcBuilder AddScfAreas(this IMvcBuilder builder)
        {
            //遍历所有程序集进行注册

            var dt1 = SystemTime.Now;

            var cacheStragegy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
            using (cacheStragegy.BeginCacheLock("Senparc.Scf.Core.Areas.AreaRegister", "RegisterScfAreas"))
            {
                if (RegisterAreasFinished == true)
                {
                    return builder;
                }

                //查找所有扩展缓存B
                var scanTypesCount = 0;

                var assembiles = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assembly in assembiles)
                {
                    try
                    {
                        scanTypesCount++;
                        var areaRegisterTypes = assembly.GetTypes()
                                    .Where(z => z.GetInterface("IAreaRegister") != null)
                                    .ToArray();

                        foreach (var registerType in areaRegisterTypes)
                        {
                            var register = Activator.CreateInstance(registerType, true) as IAreaRegister;
                            register.AuthorizeConfig(builder);//进行注册
                        }
                    }
                    catch (Exception ex)
                    {
                        SenparcTrace.SendCustomLog("RegisterAllAreas() 自动扫描程序集报告（非程序异常）：" + assembly.FullName, ex.ToString());
                    }
                }

                RegisterAreasFinished = true;

                var dt2 = SystemTime.Now;
                Console.WriteLine($"RegisterAllAreas 用时：{(dt2 - dt1).TotalMilliseconds}ms");

                return builder;
            }
        }
    }
}