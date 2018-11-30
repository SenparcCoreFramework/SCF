using Senparc.Core.Cache.Lock;
using System;
using System.Collections.Generic;

namespace Senparc.Core.Cache
{

    public interface IBaseCacheStrategy
    {
        /// <summary>
        /// 开始一个同步锁
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="key"></param>
        /// <param name="retryCount"></param>
        /// <param name="retryDelay"></param>
        /// <returns></returns>
        ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0,
                    TimeSpan retryDelay = new TimeSpan());

    }

    /// <summary>
    /// 公共缓存策略接口
    /// </summary>
    public interface IBaseCacheStrategy<T> : IBaseCacheStrategy where T : class/*, new()*/
    {
        /// <summary>
        /// 整个Cache集合的Key
        /// </summary>
        string CacheSetKey { get; set; }

        ///// <summary>
        ///// 缓存强类型
        ///// </summary>
        //Type CacheSetType { get; set; }


        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        void InsertToCache(string key, T value);
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">单位：分</param>
        void InsertToCache(string key, T value, int timeout);

        /// <summary>
        /// 移除指定缓存键的对象
        /// </summary>
        /// <param name="key">缓存键</param>
        void RemoveFromCache(string key);

        /// <summary>
        /// 返回指定缓存键的对象
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        T Get(string key);

        /// <summary>
        /// 获取所有细信息
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 获取指定Key下所有的子Key的Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IList<T> GetAll(string key);
        /// <summary>
        /// 检查是否存在Key及对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckExisted(string key);

        /// <summary>
        /// 获取缓存集合总数（注意：每个缓存框架的计数对象不一定一致！）
        /// </summary>
        /// <returns></returns>
        long GetCount();


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        void UpdateData(string key, T obj);


    }
}
