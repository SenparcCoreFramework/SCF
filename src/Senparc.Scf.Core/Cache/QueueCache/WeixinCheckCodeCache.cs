using System;

namespace Senparc.Scf.Core.Cache.QueueCache
{
    //public interface IWeixinCheckCodeCache : IQueueCache<QueueCacheData<string>>
    //{
    //}

    [Serializable]
    public class WeixinCheckCodeCache : QueueCache<QueueCacheData<string>>/*, IWeixinCheckCodeCache*/
    {
        private const string cacheKey = "WeixinCheckCodeCache";
        private const int timeoutSeconds = 3 * 60;
        public WeixinCheckCodeCache()
            : base(cacheKey, timeoutSeconds)
        {

        }

        public override QueueCacheData<QueueCacheData<string>> Get(string key, bool removeDataWhenExist = true)
        {
            var value = base.Get(key, removeDataWhenExist);
            if (value != null)
            {
                base.Remove(key);//一次性有效
            }
            return value;
        }
    }
}
