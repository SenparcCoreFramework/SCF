using System;

namespace Senparc.Scf.Core.Cache
{
    [Serializable]
    public class OAuthCodeData
    {

        public int AccountId { get; set; }
        public int AppId { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }

        public OAuthCodeData(int accountId, int appId, string appKey, string appSecret)
        {
            AccountId = accountId;
            AppId = appId;
            AppKey = appKey;
            AppSecret = appSecret;
        }
    }

    /// <summary>
    /// 登录许可缓存（缓存数据：UserId）
    /// </summary>
    public interface IOAuthCodeCache : IQueueCache<OAuthCodeData>
    {

    }

    [Serializable]
    public class OAuthCodeCache : QueueCache<OAuthCodeData>, IOAuthCodeCache
    {
        private const string cacheKey = "OAuthCodeCache";
        private const int timeoutSeconds = 5 * 60;
        public OAuthCodeCache()
            : base(cacheKey, timeoutSeconds)
        {

        }

        public override string CreateKey()
        {
            return Guid.NewGuid().ToString("n");
        }

        public override QueueCacheData<OAuthCodeData> Get(string key, bool removeDataWhenExist = true)
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
