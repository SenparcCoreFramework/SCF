using System;
using Senparc.Core.Cache.BaseCache;

namespace Senparc.Core.Cache
{
    public static class MethodCache
    {
        public static T GetMethodCache<T>(string cacheKey, Func<T> func, int timeoutSeconds) where T : class
        {

            cacheKey = cacheKey.ToUpper();

            var cache = CacheStrategyFactory.GetCacheStrategy<T>();

            T result;

            if (!cache.CheckExisted(cacheKey))
            {
                cache.InsertToCache(cacheKey, func(), //每次储存的是重新执行过的最新的结果
                    timeoutSeconds * 60);
            }

            result = cache.Get(cacheKey);//输出结果

            return result;
        }

        public static T GetMethodCache<T>(string cacheKey, Func<T> func) where T : class
        {
            return GetMethodCache(cacheKey, func, 60 * 60);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void ClearMethodCache<T>(string cacheKey) where T : class //虽然这边的T不需要传入，不过为了拿到CacheStrategy仍然需要提供
        {
            cacheKey = cacheKey.ToUpper();

            var cache = CacheStrategyFactory.GetCacheStrategy<T>();

            cache.RemoveFromCache(cacheKey);
        }
    }
}
