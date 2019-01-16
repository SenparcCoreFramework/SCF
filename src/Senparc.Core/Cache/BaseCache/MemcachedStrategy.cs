using System;
using System.Collections.Generic;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Core.Cache.Lock;
//using Memcached.ClientLibrary;
using Senparc.Core.Config;
using Senparc.Core.Enums;
using Senparc.Log;
using Senparc.Utility;

namespace Senparc.Core.Cache
{
    public class MemcachedStrategy<T> : IBaseCacheStrategy<T> where T : class/*, new()*/
    {
        //private static volatile System.Web.Caching.Cache cache = HttpRuntime.Cache;
        private MemcachedClient _cache;

        private MemcachedClientConfiguration _config;
        private static Dictionary<string, int> _serverlist = SiteConfig.MemcachedAddresses;
        //private const string KEY_FORMAT = "{0}{1}";

        #region 单例

        //静态SearchCache
        public static MemcachedStrategy<T> Instance
        {
            get
            {
                return Nested.instance;//返回Nested类中的静态成员instance
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            //将instance设为一个初始化的BaseCacheStrategy新实例
            internal static readonly MemcachedStrategy<T> instance = new MemcachedStrategy<T>();
        }

        #endregion

        private static MemcachedClientConfiguration GetMemcachedClientConfiguration()
        {
            ILoggerFactory loggerFactory = SenparcDI.GetService<ILoggerFactory>();
            IOptions<MemcachedClientOptions> optionsAccessor = SenparcDI.GetService<IOptions<MemcachedClientOptions>>();


            //每次都要新建
            var config = new MemcachedClientConfiguration(loggerFactory, optionsAccessor);
            foreach (var server in _serverlist)
            {
                config.Servers.Add(new DnsEndPoint(server.Key, server.Value));
            }
            config.Protocol = MemcachedProtocol.Binary;

            return config;
        }

        static MemcachedStrategy()
        {
            // //初始化memcache服务器池
            //SockIOPool pool = SockIOPool.GetInstance();
            ////设置Memcache池连接点服务器端。
            //pool.SetServers(serverlist);
            ////其他参数根据需要进行配置 

            //pool.InitConnections = 3;
            //pool.MinConnections = 3;
            //pool.MaxConnections = 5;

            //pool.SocketConnectTimeout = 1000;
            //pool.SocketTimeout = 3000;

            //pool.MaintenanceSleep = 30;
            //pool.Failover = true;

            //pool.Nagle = false;
            //pool.Initialize();

            //cache = new MemcachedClient();
            //cache.EnableCompression = false;
            try
            {
                //config.Authentication.Type = typeof(PlainTextAuthenticator);
                //config.Authentication.Parameters["userName"] = "username";
                //config.Authentication.Parameters["password"] = "password";
                //config.Authentication.Parameters["zone"] = "zone";//domain?   ——Jeffrey 2015.10.20
                DateTime dt1 = DateTime.Now;
                var config = GetMemcachedClientConfiguration();
                ILoggerFactory loggerFactory = SenparcDI.GetService<ILoggerFactory>();
                var cache = new MemcachedClient(loggerFactory, config);

                var testKey = Guid.NewGuid().ToString();
                var testValue = Guid.NewGuid().ToString();
                cache.Store(StoreMode.Set, testKey, testValue);
                var storeValue = cache.Get(testKey);
                if (storeValue as string != testValue)
                {
                    throw new Exception("MemcachedStrategy失效，没有计入缓存！");
                }
                cache.Remove(testKey);
                DateTime dt2 = DateTime.Now;
                LogUtility.Cache.Error($"MemcachedStrategy正常启用，启动及测试耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                SiteConfig.CacheType = CacheType.Local;//强制切换到本地缓存状态
                LogUtility.SystemLogger.ErrorFormat($"MemcachedStrategy静态构造函数异常：{ex.Message}", ex);

                throw new Exception("系统繁忙，请稍后再试！");
            }
        }

        public MemcachedStrategy()
        {
            //以下为测试代码：
            //var services = new ServiceCollection();
            //var provider = services.BuildServiceProvider();
            //ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>();
            //IOptions<MemcachedClientOptions> optionsAccessor = provider.GetService<IOptions<MemcachedClientOptions>>();

            ////_config = GetMemcachedClientConfiguration();
            //var config = new MemcachedClientConfiguration(loggerFactory, optionsAccessor);
        }

        private string GetFinalKey(string key)
        {
            return $"{CacheSetKey}{key}";
        }

        #region ICacheStrategy 成员

        public string CacheSetKey { get; set; }
        public Type CacheSetType { get; set; }

        public ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0, TimeSpan retryDelay = new TimeSpan())
        {
            throw new NotImplementedException("此方法不可用");
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
            //TODO：加了绝对过期时间就会立即失效（再次获取后为null），memcache低版本的bug
            var result = _cache.Store(StoreMode.Set, cacheKey, obj, TimeSpan.FromMinutes(timeout));
            if (result == false)
            {
                LogUtility.Cache.ErrorFormat("InsertToCache失败！key：{0}", key);
            }

#if DEBUG
            var value = _cache.Get(cacheKey);
#endif
        }

        public virtual void RemoveFromCache(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            var cacheKey = GetFinalKey(key);
            _cache.Remove(cacheKey);
        }

        public virtual T Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var cacheKey = GetFinalKey(key);
            return _cache.Get(cacheKey) as T;
        }

        public IList<T> GetAll()
        {
            return null;//TODO:临时
        }

        public bool CheckExisted(string key)
        {
            object obj;
            if (_cache.TryGet(key, out obj))
            {
                return true;
            }
            return false;
        }

        public long GetCount()
        {
            throw new NotImplementedException();
        }

        public void UpdateData(string key, T obj)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll(string key)
        {
            throw new NotImplementedException();
        }

        //public int GetCountForType()
        //{
        //    return _cache.
        //}

        #endregion
    }
}
