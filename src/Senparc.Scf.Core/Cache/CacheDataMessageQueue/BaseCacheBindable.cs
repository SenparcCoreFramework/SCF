using System;
using System.Runtime.CompilerServices;
using Senparc.CO2NET.Cache;
using Senparc.Scf.Core.Cache.BaseCache;
using Senparc.Scf.Log;
using Senparc.Weixin.Entities;

namespace Senparc.Scf.Core.Cache
{
    /// <summary>
    /// 所有需要分布式缓存的实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseCacheBindable<T> : BindableBase where T : class, ICacheData, new()
    {
        ///// <summary>
        ///// 缓存键
        ///// </summary>
        //public abstract object Key { get; }

        protected string GenerateKey_Name { get; set; }
        protected string GenerateKey_ActionName { get; set; }


        protected void BaseCacheBindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender == null)
            {
                LogUtility.Cache.ErrorFormat("BaseCacheBindable发生错误，不是BaseCacheBindable类型。当前参数类型：{0}", sender.GetType());
                return;
            }

            var objCacheData = sender as ICacheData;
            if (objCacheData == null)
            {
                LogUtility.Cache.ErrorFormat("BaseCacheBindable发生错误，没有实现ICacheData接口。当前参数类型：{0}", sender.GetType());
                return;
            }

            if (objCacheData.Key == null)
            {
                LogUtility.Cache.ErrorFormat("BaseCacheBindable发生错误，Key为空。当前参数类型：{0}", sender.GetType());
                return;
            }

            var mqKey = CacheDataMessageQueue.GenerateKey("SenparcCache", sender.GetType(), objCacheData.Key as string, "UpdateCache");

            //获取对应Container的缓存相关

            //加入消息列队，每过一段时间进行自动更新，防止属性连续被编辑，短时间内反复更新缓存。
            CacheDataMessageQueue mq = new CacheDataMessageQueue();
            mq.Add(mqKey, () =>
            {
                var cacheStragegy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
                //var cacheKey = objCacheData.Key;
                objCacheData.CacheTime = DateTime.Now;//记录缓存时间
                cacheStragegy.Set(objCacheData.Key, objCacheData as T);

                //var cacheKey = ContainerHelper.GetCacheKey(this.GetType());
                //containerBag.CacheTime = DateTime.Now;//记录缓存时间
                //containerCacheStragegy.UpdateContainerBag(cacheKey, containerBag);
            });
        }


        /// <summary>
        /// 设置Container属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetContainerProperty(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);
            return result;
        }

        public BaseCacheBindable()
        {
            base.PropertyChanged += BaseCacheBindable_PropertyChanged;
        }
    }
}
