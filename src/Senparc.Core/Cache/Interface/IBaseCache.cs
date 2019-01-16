using System;
using Senparc.CO2NET.Cache;
namespace Senparc.Core.Cache
{
    public interface IBaseCache<T>
       where T : class, new()
    {
        IBaseObjectCacheStrategy Cache { get; set; }
        /// <summary>
        /// Data不能在Update()方法中调用，否则会引发循环调用。Update()方法中应该使用SetData()方法
        /// </summary>
        T Data { get; set; }
        DateTime CacheTime { get; set; }
        DateTime CacheTimeOut { get; set; }
        void RemoveCache();
        void SetData(T value, int timeOut, BaseCache<T>.UpdateWithBataBase updateWithDatabases);
        T Update();
        void UpdateToDatabase(T obj);

        ///// <summary>
        ///// 更新到缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //bool UpdateToCache(string key, T obj);
    }
}
