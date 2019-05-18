using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//using WURFL;

namespace Senparc.Core.Models
{

    #region 数据库实体扩展


    [Serializable]
    public partial class FullSystemConfigBase : BaseFullEntity<SystemConfig>
    {
        [AutoSetCache]
        public string SystemName { get; set; }

        public override void CreateEntity(SystemConfig entity)
        {
            base.CreateEntity(entity);
        }
    }

    [Serializable]
    public class FullSystemConfig : FullSystemConfigBase
    {
        [AutoSetCache]
        public string MchId { get; set; }
        [AutoSetCache]
        public string MchKey { get; set; }
        [AutoSetCache]
        public string TenPayAppId { get; set; }


        public override void CreateEntity(SystemConfig entity)
        {
            base.CreateEntity(entity);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public partial class FullAccountBase : BaseFullEntity<Account>
    {
        public override string Key => UserName;

        [AutoSetCache]
        public int Id { get; set; }

        [AutoSetCache]
        public string UserName { get; set; }
        public DateTime LastActiveTime { get; set; }
        public DateTime LastOpenPageTime { get; set; }
        public string LastOpenPageUrl { get; set; }
        public string LastActiveUserAgent { get; set; }
        ///// <summary>
        ///// 最近一次活动的客户端设备信息
        ///// </summary>
        //public IDevice LastActiveDevice
        //{
        //    get
        //    {
        //        if (!LastActiveUserAgent.IsNullOrEmpty())
        //        {
        //            var wurflManager = WurflUtility.WurflManager;
        //            var device = wurflManager.GetDeviceForRequest(LastActiveUserAgent);
        //            return device;
        //        }
        //        return null;
        //    }
        //}

        public bool IsLogined => Id > 0 && !UserName.IsNullOrEmpty() && (DateTime.Now - LastActiveTime).TotalMinutes < 2;

        /// <summary>
        /// 强制退出登录
        /// </summary>
        public bool ForceLogout { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnReadMessageCount { get; set; }

        /// <summary>
        /// 已经输入过验证码（如果需要加强验证，可以加上次输入验证码的时间以及token）
        /// </summary>
        public bool CheckCodePassed { get; set; }

        /// <summary>
        /// 在线图标
        /// </summary>
        public string OnlineImg => $"/Content/Images/{(IsLogined ? "online" : "offline")}.png";

        public FullAccountBase()
        {
            LastActiveTime = DateTime.MinValue;
            LastOpenPageTime = DateTime.MinValue;
        }
    }

    [Serializable]
    public class FullAccount : FullAccountBase
    {
        [AutoSetCache]
        public string Password { get; set; }

        [AutoSetCache]
        public string PasswordSalt { get; set; }

        [AutoSetCache]
        public string NickName { get; set; }

        [AutoSetCache]
        public string RealName { get; set; }

        [AutoSetCache]
        public string Email { get; set; }

        [AutoSetCache]
        public bool? EmailChecked { get; set; }

        [AutoSetCache]
        public string Phone { get; set; }

        [AutoSetCache]
        public bool? PhoneChecked { get; set; }

        [AutoSetCache]
        public string WeixinOpenId { get; set; }

        [AutoSetCache]
        public string PicUrl { get; set; }

        [AutoSetCache]
        public string HeadImgUrl { get; set; }

        [AutoSetCache]
        public decimal Package { get; set; }

        [AutoSetCache]
        public decimal Balance { get; set; }

        [AutoSetCache]
        public decimal LockMoney { get; set; }

        [AutoSetCache]
        public byte Sex { get; set; }

        [AutoSetCache]
        public string QQ { get; set; }

        [AutoSetCache]
        public string Country { get; set; }

        [AutoSetCache]
        public string Province { get; set; }

        [AutoSetCache]
        public string City { get; set; }

        [AutoSetCache]
        public string District { get; set; }

        [AutoSetCache]
        public string Address { get; set; }

        [AutoSetCache]
        public string Note { get; set; }

        [AutoSetCache]
        public System.DateTime ThisLoginTime { get; set; }

        [AutoSetCache]
        public string ThisLoginIp { get; set; }

        [AutoSetCache]
        public System.DateTime LastLoginTime { get; set; }

        [AutoSetCache]
        public string LastLoginIP { get; set; }

        [AutoSetCache]
        public System.DateTime AddTime { get; set; }

        [AutoSetCache]
        public decimal Points { get; set; }

        [AutoSetCache]
        public DateTime? LastWeixinSignInTime { get; set; }

        [AutoSetCache]
        public int WeixinSignTimes { get; set; }

        [AutoSetCache]
        public string WeixinUnionId { get; set; }

        /// <summary>
        /// 账户显示名称
        /// </summary>
        public string DisplayName => NickName ?? UserName;

        public override void CreateEntity(Account entity)
        {
            base.CreateEntity(entity);
        }

    }


    #endregion

    #region Email

    /// <summary>
    /// 自动发送
    /// </summary>
    public class AutoSendEmail
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string UserName { get; set; }

        public DateTime LastSendTime { get; set; }

        public int SendCount { get; set; }
    }

    /// <summary>
    /// 自动发送完成
    /// </summary>
    public class AutoSendEmailBak
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string UserName { get; set; }

        public DateTime SendTime { get; set; }
    }

    public class EmailUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public bool NeedCredentials { get; set; }

        public string Note { get; set; }
    }

    #endregion

    #region XML Config格式
    /// <summary>
    /// Email
    /// </summary>
    public class XmlConfig_Email
    {
        public string ToUse { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Holders { get; set; }

        public DateTime UpdateTime { get; set; }
    }

    #endregion

    #region 省、市、区XML数据格式
    [DataContract]
    [Serializable]
    public class AreaXML_Provinces
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 地区代码
        /// </summary>
        [DataMember]
        public string DivisionsCode { get; set; }

        /// <summary>
        /// 缩写（去掉“省”“市”“自治区”等）
        /// </summary>
        [DataMember]
        public string ShortName { get; set; }

        public AreaXML_Provinces(int id, string provinceName, string divisionsCode, string shortName)
        {
            this.ID = id;
            this.ProvinceName = provinceName;
            this.DivisionsCode = divisionsCode;
            this.ShortName = shortName;
        }
    }

    [DataContract]
    [Serializable]
    public class AreaXML_Cities
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int PID { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public int MaxShopId { get; set; }

        public AreaXML_Cities(int id, int pID, string cityName, string zipCode, string cityCode, int maxShopId)
        {
            this.ID = id;
            this.PID = pID;
            this.CityName = cityName;
            this.ZipCode = zipCode;
            this.CityCode = cityCode;
            this.MaxShopId = maxShopId;
        }
    }

    [DataContract]
    [Serializable]
    public class AreaXML_Districts
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int CID { get; set; }

        [DataMember]
        public string DistrictName { get; set; }

        public AreaXML_Districts(int id, int cID, string districtName)
        {
            this.ID = id;
            this.CID = cID;
            this.DistrictName = districtName;
        }
    }

    #endregion
}

