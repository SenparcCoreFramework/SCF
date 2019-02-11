using Senparc.CO2NET.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.Core.AssembleScan
{
    public static class AssembleScanHelper
    {
        /// <summary>
        /// 所有扫描方法的集合
        /// </summary>
        public static List<AssembleScanItem> ScanAssamblesActions { get; set; } = new List<AssembleScanItem>();

        private static object _scanLock = new object();

        /// <summary>
        /// 添加扫描项目
        /// </summary>
        /// <param name="action">扫描过程</param>
        /// <param name="runScanNow">是否立即扫描</param>
        public static void AddAssembleScanItem(Action<Assembly> action, bool runScanNow)
        {
            ScanAssamblesActions.Add(new AssembleScanItem(action));

            if (runScanNow)
            {
                RunScan();//立即扫描
            }
        }

        /// <summary>
        /// 执行扫描
        /// </summary>
        public static void RunScan()
      {
            var dt1 = SystemTime.Now;

            lock (_scanLock)
            {
                //查找所有扩展缓存B
                var scanTypesCount = 0;

                var assembiles = AppDomain.CurrentDomain.GetAssemblies();
                var toScanItems = ScanAssamblesActions.Where(z => z.ScanFinished == false).ToList();
                foreach (var assembly in assembiles)
                {
                    try
                    {
                        scanTypesCount++;
                        foreach (var scanItem in toScanItems)
                        {
                            scanItem.Run(assembly);//执行扫描过程
                        }
                    }
                    catch (Exception ex)
                    {
                        SenparcTrace.SendCustomLog("ScanAssambles() 自动扫描程序集报告（非程序异常）：" + assembly.FullName, ex.ToString());
                    }
                }

                toScanItems.ForEach(z => z.Finished());//标记结束

                var dt2 = SystemTime.Now;
                SenparcTrace.SendCustomLog("ScanAssambles", $"RegisterAllAreas 用时：{(dt2 - dt1).TotalMilliseconds}ms");

                return;
            }
        }
    }
}
