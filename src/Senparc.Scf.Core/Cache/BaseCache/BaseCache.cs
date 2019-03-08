using Senparc.CO2NET;
using Senparc.CO2NET.Cache;
using Senparc.Scf.Core.Cache.BaseCache;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Utility;
using System;

namespace Senparc.Scf.Core.Cache
{
    public abstract class BaseCache<T> : IBaseCache<T> where T : class, new()
    {
        protected virtual bool UpdateToCache(string key, T obj)
        {
            Cache.Set(key, obj);
            return true;
        }

        public BaseCache() { }

        public delegate void UpdateWithBataBase(T obj);

        protected ISqlClientFinanceData _db;

        protected string CacheKey;
        private T _data;

        public string CacheSetKey { get; private set; }

        public DateTime CacheTime { get; set; }
        public DateTime CacheTimeOut { get; set; }

        //private ICacheStrategy _cache;

        /// <summary>
        /// 缓存策略。
        /// 请尽量不要再BaseCache以外调用这个对象的方法，尤其Cache的Key在DictionaryCache中是会被重新定义的
        /// </summary>
        public IBaseObjectCacheStrategy Cache { get; set; }
        /// <summary>
        /// 超时时间，1400分钟为1天。
        /// </summary>
        public int TimeOut { get; set; }

        public BaseCache(string cacheKey)
            : this(cacheKey, null)
        { }

        public BaseCache(string cacheKey, ISqlClientFinanceData db)
        {
            CacheKey = cacheKey;

            _db = db ?? SenparcDI.GetService<ISqlClientFinanceData>(); //
            if (TimeOut == 0)
            {
                TimeOut = 1440;
            }

            Cache = CacheStrategyFactory.GetObjectCacheStrategyInstance();
            this.CacheSetKey = cacheKey;//设置缓存集合键，必须提供
        }

        /// <summary>
        /// Data不能在Update()方法中调用，否则会引发循环调用。Update()方法中应该使用SetData()方法
        /// Data只适用于简单类型，如果缓存类型为列表，则不适用
        /// </summary>
        public virtual T Data
        {
            get
            {
                if (_data != null)
                {
                    return _data;
                }

                if (Cache == null)
                {
                    var msg = "Cache==null!系统调试记录cache长久以来的一个bug。";
                    throw new Exception(msg);
                }

                if (Cache.Get(CacheKey) == null)
                {
                    _data = this.Update();
                }
                return Cache.Get<T>(CacheKey);
            }
            set => Cache.Set(CacheKey, value, TimeSpan.FromMinutes(TimeOut));
        }

        /// <summary>
        /// 设置整个缓存数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <param name="updateWithDatabases"></param>
        public virtual void SetData(T value, int timeOut, UpdateWithBataBase updateWithDatabases)
        {
            Cache.Set(CacheKey, value, TimeSpan.FromMinutes(timeOut));

            //记录缓存时间
            this.CacheTime = DateTime.Now;
            this.CacheTimeOut = this.CacheTime.AddMinutes(timeOut);

            updateWithDatabases?.Invoke(value);
        }

        public virtual void RemoveCache()
        {
            Cache.RemoveFromCache(CacheKey);
        }

        public virtual T Update()
        {
            return null;
        }

        public virtual void UpdateToDatabase(T obj)
        {
        }

    }
}
