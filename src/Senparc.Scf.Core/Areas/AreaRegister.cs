using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.AssembleScan;
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

        public static object AddScfAreasLock = new object();

        /// <summary>
        /// 自动注册所有 Area
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddScfAreas(this IMvcBuilder builder)
        {
            AssembleScanHelper.AddAssembleScanItem(assembly =>
            {
                try
                {
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
                    SenparcTrace.SendCustomLog("AddScfAreas() 自动扫描程序集报告（非程序异常）：" + assembly.FullName, ex.ToString());
                }
            }, false);
            return builder;
        }
    }
}