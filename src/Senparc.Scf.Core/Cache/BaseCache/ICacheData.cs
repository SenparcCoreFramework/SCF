using System;

namespace Senparc.Scf.Core.Cache
{
    public interface ICacheData
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        string Key { get; }

        DateTime CacheTime { get; set; }
    }
}
