using NLog;
using Senparc.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Log
{
  public static  class NLogExtension
    {
        /// <summary>
        /// 记录错误信息的扩展方法
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="stringFormat"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(this Logger logger, string stringFormat, params object[] args)
        {
            logger.Error(stringFormat.With(args));
        }
    }
}
