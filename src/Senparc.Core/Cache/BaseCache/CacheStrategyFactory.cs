using System;
using Senparc.CO2NET;
using Senparc.Core.Config;
using Senparc.Core.Enums;

namespace Senparc.Core.Cache.BaseCache
{
    /// <summary>
    /// 缓存策略工厂
    /// </summary>
    public class CacheStrategyFactory
    {
        public static IBaseCacheStrategy<T> GetCacheStrategy<T>() where T : class/*, new()*/
        {
            IBaseCacheStrategy<T> cache;
            switch (SiteConfig.CacheType)
            {
                case CacheType.Local:
                    cache = LocalCacheStrategy<T>.Instance;
                    break;
                case CacheType.Memcached:
                    cache = MemcachedStrategy<T>.Instance;
                    break;
                case CacheType.Redis:
                    cache = RedisStrategy<T>.Instance;
                    break;
                default:
                    throw new Exception("未处理的CacheType类型：" + SiteConfig.CacheType.ToString());
            }
            return cache;
        }
    }
}
