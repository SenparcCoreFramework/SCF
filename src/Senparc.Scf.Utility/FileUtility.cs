using System;

namespace Senparc.Scf.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// 获取随机文件名
        /// </summary>
        /// <returns></returns>
        public static string GetRandomFileName()
        {
            return $"{DateTime.Now.Ticks}{Guid.NewGuid().ToString("n").Substring(0, 8)}";
        }
    }
}