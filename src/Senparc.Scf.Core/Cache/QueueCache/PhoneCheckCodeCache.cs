using System;

namespace Senparc.Scf.Core.Cache
{
    [Serializable]
    public class PhoneCheckCodeData
    {
        public string Phone { get; set; }

        public PhoneCheckCodeData(string phone)
        {
            Phone = phone;
        }
    }

    ///// <summary>
    ///// 手机验证码
    ///// </summary>
    //public interface IPhoneCheckCodeCache : IQueueCache<PhoneCheckCodeData>
    //{

    //}

    /// <summary>
    /// 手机验证码
    /// </summary>
    [Serializable]
    public class PhoneCheckCodeCache : QueueCache<PhoneCheckCodeData>/*, IPhoneCheckCodeCache*/
    {
        private const string cacheKey = "PhoneCheckCodeCache";
        private const int timeoutSeconds = 10 * 60;
        public PhoneCheckCodeCache()
            : base(cacheKey, timeoutSeconds)
        {

        }

        public override string CreateKey()
        {
            string key = new Random().Next(100000,999999).ToString("######");
            return key;
        }

        public override QueueCacheData<PhoneCheckCodeData> Get(string key, bool removeDataWhenExist = true)
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
