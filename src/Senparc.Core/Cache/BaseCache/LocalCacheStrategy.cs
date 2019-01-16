using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Senparc.CO2NET;
using Senparc.Core.Cache.Lock;
using Senparc.Utility;

namespace Senparc.Core.Cache
{

    public interface ILocalCacheStrategy : IBaseCacheStrategy
    {
    }

    public partial class LocalCacheStrategy<T> : IBaseCacheStrategy<T>, ILocalCacheStrategy where T : class/*, new()*/
    {

        private static volatile IMemoryCache _cache = null;

        /// <summary>
        /// 当前缓存
        /// </summary>
        private static /*volatile*/ IMemoryCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = SenparcDI.GetService<IMemoryCache>();
                }
                return _cache;
            }
        }

        ///// <summary>
        ///// 默认到期时间
        ///// </summary>
        //private static int _timeout = 60;//单位：分

        #region 单例

        /// <summary>
        /// BaseCacheStrategy的构造函数
        /// </summary>
        LocalCacheStrategy()
        {
        }

        //静态BaseCacheStrategy
        public static LocalCacheStrategy<T> Instance
        {
            get
            {
                return Nested.instance; //返回Nested类中的静态成员instance
            }
        }

        class Nested
        {
            static Nested()
            {
            }

            //将instance设为一个初始化的BaseCacheStrategy新实例
            internal static readonly LocalCacheStrategy<T> instance = new LocalCacheStrategy<T>();
        }

        #endregion

        #region ICacheStrategy 成员

        public string CacheSetKey { get; set; }

        public ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0, TimeSpan retryDelay = new TimeSpan())
        {
            return new LocalCacheLock(this, resourceName, key, retryCount, retryDelay).LockNow();
        }

        public virtual void InsertToCache(string key, T obj)
        {
            this.InsertToCache(key, obj, 1440 /*默认分钟*/);
        }

        public virtual void InsertToCache(string key, T obj, int timeout)
        {
            if (string.IsNullOrEmpty(key) || obj == null)
            {
                return;
            }

            Cache.Set(key, obj, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(timeout)),
                Priority = CacheItemPriority.High
            });
        }


        public virtual void RemoveFromCache(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Cache.Remove(key);
        }

        public virtual T Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return Cache.Get(key) as T;
        }

        public IList<T> GetAll()
        {
            throw new Exception("当前缓存策略不支持此方法！ GetAll()");
            ////TODO:临时方案
            //List<string> cacheKeys = new List<string>();

            //IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
            //while (cacheEnum.MoveNext())
            //{
            //    cacheKeys.Add(cacheEnum.Key.ToString());
            //}

            //var objectKeys = cacheKeys.Where(z => z.StartsWith(CacheSetKey));
            //List<T> objects = new List<T>();
            //foreach (var objectKey in objectKeys)
            //{
            //    objects.Add(Cache[objectKey] as T);
            //}
            //return objects;
        }

        public IList<T> GetAll(string key)
        {
            throw new Exception("当前缓存策略不支持此方法！ GetAll()");

            //List<string> cacheKeys = new List<string>();
            //IDictionaryEnumerator cacheEnum = Cache.GetEnumerator();
            //while (cacheEnum.MoveNext())
            //{
            //    cacheKeys.Add(cacheEnum.Key.ToString());
            //}
            //var objectKeys = cacheKeys.Where(z => z.StartsWith(key)).ToList();
            //List<T> objects = new List<T>();
            //foreach (var objectKey in objectKeys)
            //{
            //    objects.Add(Cache[objectKey] as T);
            //}
            //return objects;
        }


        public bool CheckExisted(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// 将返回所有类型的缓存数量
        /// </summary>
        /// <returns></returns>
        public int GetCountForType()
        {
            throw new Exception("当前缓存策略不支持此方法！ GetCountForType()");

            //return Cache.Count;


            //List<string> cacheKeys = new List<string>();
            //IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            //while (cacheEnum.MoveNext())
            //{
            //    cacheKeys.Add(cacheEnum.Key.ToString());
            //}
            //Collection<string> s = 
            //var needKeys = cacheKeys.Where(z => z.StartsWith(CacheSetKey));
            //return needKeys.Count();
        }

        public long GetCount()
        {
            throw new Exception("当前缓存策略不支持此方法！ GetCount()");

            //return Cache.Count;
        }

        public void UpdateData(string key, T obj)
        {
            InsertToCache(key, obj);
        }

        #endregion
    }
}