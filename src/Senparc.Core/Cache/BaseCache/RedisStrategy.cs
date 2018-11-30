using Senparc.Core.Cache.BaseCache.Redis;
using Senparc.Core.Cache.Lock;
using Senparc.Log;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Senparc.Core.Cache
{
    public interface IRedisStrategy : IBaseCacheStrategy
    {
        StackExchange.Redis.ConnectionMultiplexer _client { get; set; }
    }

    public class RedisStrategy<T> : IBaseCacheStrategy<T>, IRedisStrategy where T : class/*, new()*/
    {
        //private static volatile System.Web.Caching.Cache cache = HttpRuntime.Cache;
        //private MemcachedClient _cache;
        //private RedisConfigInfo _config;
        public ConnectionMultiplexer _client { get; set; }
        private IDatabase _cache;

        #region 单例

        //静态SearchCache
        public static RedisStrategy<T> Instance => Nested.instance; //返回Nested类中的静态成员instance

        private class Nested
        {
            static Nested()
            {
            }

            //将instance设为一个初始化的BaseCacheStrategy新实例
            internal static readonly RedisStrategy<T> instance = new RedisStrategy<T>();
        }

        #endregion

        static RedisStrategy()
        {
            DateTime dt1 = DateTime.Now;
            var manager = RedisManager.Manager;
            var cache = manager.GetDatabase();

            var testKey = Guid.NewGuid().ToString();
            var testValue = Guid.NewGuid().ToString();
            cache.StringSet(testKey, testValue);
            var storeValue = cache.StringGet(testKey);
            if (storeValue != testValue)
            {
                throw new Exception("RedisStrategy失效，没有计入缓存！");
            }

            cache.StringSet(testKey, (string)null);
            DateTime dt2 = DateTime.Now;
            LogUtility.Cache.Info($"RedisStrategy正常启用，启动及测试耗时：{(dt2 - dt1).TotalMilliseconds}ms");
        }

        public RedisStrategy()
        {
            //_config = RedisConfigInfo.GetConfig();
            _client = RedisManager.Manager;
            _cache = _client.GetDatabase();
        }

        ~RedisStrategy()
        {
            _client.Dispose(); //释放
            //GC.SuppressFinalize(_client);
        }

        private string GetFinalKey(string key)
        {
            return $"{CacheSetKey}:{key}";
            //return "{0}".With(CacheSetKey);
        }

        #region ICacheStrategy 成员

        public string CacheSetKey { get; set; }
        public Type CacheSetType { get; set; }

        /// <summary>
        /// 获取 Server 对象
        /// </summary>
        /// <returns></returns>
        private IServer GetServer()
        {
            //https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/KeysScan.md
            var server = _client.GetServer(_client.GetEndPoints()[0]);
            return server;
        }

        public ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0, TimeSpan retryDelay = new TimeSpan())
        {
            return new RedisCacheLock<T>(this, resourceName, key, retryCount, retryDelay).LockNow();
        }

        public virtual void InsertToCache(string key, T obj)
        {
            this.InsertToCache(key, obj, 1440);
        }

        public virtual void InsertToCache(string key, T obj, int timeout)
        {
            if (string.IsNullOrEmpty(key) || obj == null)
            {
                return;
            }

            var cacheKey = GetFinalKey(key);

            if (obj is IDictionary)
            {
                //Dictionary类型
            }

            _cache.StringSet(cacheKey, obj.Serialize());
            //_cache.HashSet(cacheKey, key, obj.Serialize());

#if DEBUG
            var value1 = _cache.StringGet(cacheKey); //正常情况下可以得到 //_cache.GetValue(cacheKey);
            //var value1 = _cache.HashGet(cacheKey, key); //正常情况下可以得到 //_cache.GetValue(cacheKey);
#endif
        }

        public virtual void RemoveFromCache(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            var cacheKey = GetFinalKey(key);

            //SenparcMessageQueue.OperateQueue();//延迟缓存立即生效
            _cache.KeyDelete(cacheKey); //删除键
            //_cache.HashDelete(cacheKey, key);
        }

        public virtual T Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (!CheckExisted(key))
            {
                return null;
                //InsertToCache(key, new ContainerItemCollection());
            }

            var cacheKey = GetFinalKey(key);

            var value = _cache.StringGet(cacheKey);
            //var value = _cache.HashGet(cacheKey, key);
            return StackExchangeRedisExtensions.Deserialize<T>(value);
        }

        public IList<T> GetAll()
        {
            var keys = GetServer().Keys();
            var list = new List<T>();
            foreach (var redisKey in keys)
            {
                list.Add(Get(redisKey));
            }
            return list;

            //_cache.;
            //var hash = GetHash();
            //return _cache.GetHashValues(hash);//.GetAllEntriesFromHash(hash);
        }

        /// <summary>
        /// 获取所有指定Key下所有Value【大数据量不要使用容易造成Redis线程阻塞】
        /// </summary>
        /// <param name="key">xxxxxx*</param>
        /// <returns></returns>
        public IList<T> GetAll(string key)
        {
            //TODO:consider using SCAN or sets
            var list = new List<T>();
            //var finalKey = GetFinalKey(key);
            //var hashList = _cache.HashGetAll(finalKey);
            //foreach (var hashEntry in hashList)
            //{
            //    var value = hashEntry.Value;
            //    list.Add(StackExchangeRedisExtensions.Deserialize<T>(value));
            //}

            var keys = GetServer().Keys(pattern: GetFinalKey(key + "*"));
            foreach (var redisKey in keys)
            {
                var value = _cache.StringGet(redisKey);
                list.Add(StackExchangeRedisExtensions.Deserialize<T>(value));
            }
            return list;
        }

        public bool CheckExisted(string key)
        {
            var cacheKey = GetFinalKey(key);
            return _cache.KeyExists(cacheKey);
            //return _cache.HashExists(cacheKey, key);
        }

        public int GetCountForType()
        {
            return GetServer().Keys(pattern: GetFinalKey("*")).Count();
            // return (int)_cache.HashLength(GetFinalKey(""));

            //return _client.GetAllKeys().Count(z => z.StartsWith(CacheSetKey));
        }

        public long GetCount()
        {
            var count = GetServer().Keys().Count();
            return count;
        }

        public void UpdateData(string key, T obj)
        {
            var cacheKey = GetFinalKey(key);
            _cache.StringSet(cacheKey, obj.Serialize());
            //_cache.HashSet(cacheKey, key, obj.Serialize());
        }

        #endregion
    }
}