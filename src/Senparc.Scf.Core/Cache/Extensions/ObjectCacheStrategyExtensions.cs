using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.Core.Cache.Extensions
{
    public static class ObjectCacheStrategyExtensions
    {
        public static IList<T> GetAllByPrefix<T>(this IBaseObjectCacheStrategy obj, string key)
        {
            if (obj is RedisObjectCacheStrategy)
            {
                var _obj = obj as RedisObjectCacheStrategy;
                return _obj.GetAllByPrefix<T>(key);
            }
            throw new Exception("未实现或缓存不支持前缀方式获取缓存！");
        }

        public static async Task<IList<T>> GetAllByPrefixAsync<T>(this IBaseObjectCacheStrategy obj, string key)
        {
            if (obj is RedisObjectCacheStrategy)
            {
                var _obj = obj as RedisObjectCacheStrategy;
                return await _obj.GetAllByPrefixAsync<T>(key);
            }
            throw new Exception("未实现或缓存不支持前缀方式获取缓存！");
        }
    }
}
