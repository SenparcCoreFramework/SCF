using Senparc.Core.Cache.Lock;
using System;
using System.Collections.Generic;
using Senparc.CO2NET.Cache;

namespace Senparc.Core.Cache
{

    //public interface IBaseCacheStrategy
    //{
    //    /// <summary>
    //    /// 开始一个同步锁
    //    /// </summary>
    //    /// <param name="resourceName"></param>
    //    /// <param name="key"></param>
    //    /// <param name="retryCount"></param>
    //    /// <param name="retryDelay"></param>
    //    /// <returns></returns>
    //    ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0,
    //                TimeSpan retryDelay = new TimeSpan());

    //}

    /// <summary>
    /// 公共缓存策略接口
    /// </summary>
    public interface IScfCacheStrategy : IBaseObjectCacheStrategy
    { 
        /// <summary>
        /// 整个Cache集合的Key
        /// </summary>
        string CacheSetKey { get; set; }
    }
}
