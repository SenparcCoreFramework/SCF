using Senparc.Scf.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.Core.AssembleScan
{
    public class AssembleScanItem
    {
        /// <summary>
        /// 扫描是否结束
        /// </summary>
        public bool ScanFinished { get; set; }

        /// <summary>
        /// 扫描是否成功
        /// </summary>
        public bool ScanSuccessed { get; set; }

        private Action<Assembly> _action;

        public AssembleScanItem(Action<Assembly> action)
        {
            _action = action ?? throw new ArgumentNullException("action");
        }

        private object _lock { get; set; } = new object();
        /// <summary>
        /// 执行扫描
        /// </summary>
        /// <param name="assembly"></param>
        public void Run(Assembly assembly)
        {
            lock (_lock)
            {
                try
                {
                    _action(assembly);
                    ScanSuccessed &= true;
                }
                catch (Exception ex)
                {
                    ScanSuccessed = false;
                    new SCFExceptionBase("执行程序集扫描出错", ex);
                }
                finally
                {
                }
            }
        }

        public void Finished()
        {
            ScanFinished = true;
        }
    }
}
