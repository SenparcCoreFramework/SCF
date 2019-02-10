using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using System;

namespace Senparc.Scf.Core.Cache
{
    [Serializable]
    public class MobileLoginCode
    {
        /// <summary>
        /// 完整带域名格式的用户名：<用户名>@<域名>
        /// </summary>
        public string FullDomainUserName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                if (FullDomainUserName.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                return FullDomainUserName.Split('@')[0];
            }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain
        {
            get
            {
                if (FullDomainUserName.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                return FullDomainUserName.Split('@')[1];
            }
        }
        public string Code { get; set; }
        public DateTime AddTime { get; set; }
        public string Url { get; set; }
        public bool IsValid
        {
            get { return AddTime.AddMinutes(3) > DateTime.Now; }
        }
    }

    public interface IMobileLoginCodeCache : IBaseStringDictionaryCache<MobileLoginCode>
    {

    }

    [AutoDIType(DILifecycleType.Singleton)]
    public class MobileLoginCodeCache : BaseStringDictionaryCache<MobileLoginCode>, IMobileLoginCodeCache
    {
        public MobileLoginCodeCache()
            : base("MobileLoginCodeCache", null, 120)
        {
        }

        public override MobileLoginCode InsertObjectToCache(string key)
        {
            var mobileLoginCode = new MobileLoginCode()
            {
                AddTime = DateTime.Now,
                FullDomainUserName = "",
                Code = key
            };
            return this.InsertObjectToCache(key, mobileLoginCode);
        }
    }
}
