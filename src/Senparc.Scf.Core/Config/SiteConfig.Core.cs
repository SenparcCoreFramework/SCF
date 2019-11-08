using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Utility;
using System.Collections.Generic;
using System.Threading;

namespace Senparc.Scf.Core.Config
{
    public static partial class SiteConfig
    {
        /// <summary>
        /// 网站物理路径
        /// </summary>
        public static string ApplicationPath { get; set; }
        public static string WebRootPath { get; set; }

        /// <summary>
        /// 设置
        /// </summary>
        public static SenparcCoreSetting SenparcCoreSetting
        {
            get
            {
                var scs = SenparcDI.GetService<IOptions<SenparcCoreSetting>>();
                return scs.Value;
            }
        }

        /// <summary>
        /// 是否处于Debug状态（人为手动定义）
        /// </summary>
        public static bool IsDebug => SenparcCoreSetting.IsDebug;

        /// <summary>
        /// 是否是测试站
        /// </summary>
        public static bool IsTestSite => SenparcCoreSetting.IsTestSite;

        public static Dictionary<string, int> _memcachedAddressesDic;
        public const string WEIXIN_FILTER_IGNORE = "senparcnofilter1";
        public const string WEIXIN_OFFICIAL_AVATAR_KEY = "WXoDDIC8A"; //将取前8位
        public const string WEIXIN_OFFICIAL_QR_CODE_KEY = "WX2EDIC8A"; //将取前8位
        public const string WEIXIN_APP_TOKEN_KEY = "WEIXIN_APP_TOKEN_KEY"; //微信APP Token加密
        public const long MIN_WEIXINUSERINFO_ID = 10000000000000; //最小自定义WeixinUserInfo的Id
        public const decimal PROJECTDMANDDEPOSIT = 1000; //任务默认押金
        public const string CERT_P12_ADDRESS = @"E:\";//微信支付数字证书存放地址
        /// <summary>
        /// 开发者收入比例
        /// </summary>
        public static readonly long DeveloperIncomRate = (long)0.5;

        /// <summary>
        /// TODO: 如果在系统启动时调用IHttpContextAccessor可能获取不到HttpContext！
        /// </summary>
        public static Dictionary<string, int> MemcachedAddresses
        {
            get
            {
                if (_memcachedAddressesDic == null)
                {
                    var result = new Dictionary<string, int>();
                    if (!SiteConfig.IsDebug)
                    {
                        result[SiteConfig.DEFAULT_MEMCACHED_ADDRESS_1] = SiteConfig.DEFAULT_MEMCACHED_PORT_1; //主站
                    }
                    else
                    {
                        var httpContextAccessor = SenparcDI.GetService<IHttpContextAccessor>();

                        if (SiteConfig.IsUnitTest || (IsDebug && httpContextAccessor != null && httpContextAccessor.HttpContext.Request.IsLocal()))
                        {
                            result["127.0.0.1"] = 11210; //本地
                        }
                        else if (IsTestSite && IsDebug && httpContextAccessor != null && !httpContextAccessor.HttpContext.Request.IsLocal())
                        {
                            result[SiteConfig.DEFAULT_MEMCACHED_ADDRESS_1] = SiteConfig.DEFAULT_MEMCACHED_PORT_1;
                        }
                    }

                    if (result.Count == 0)
                    {
                        result[SiteConfig.DEFAULT_MEMCACHED_ADDRESS_1] = SiteConfig.DEFAULT_MEMCACHED_PORT_1; //主站
                    }

                }
                return _memcachedAddressesDic;
            }
            set => _memcachedAddressesDic = value;

        }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public static CacheType CacheType
        {
            get => SenparcCoreSetting.CacheType;
            set => SenparcCoreSetting.CacheType = value;
        }

        //以下参数放到SiteConfig.cs中
        //public readonly static string VERSION = "1.3.2";
        //public const string GLOBAL_PASSWORD_SALT = "senparc@20131113";

        public const string VERSION = "0.0.1";
        public static string SenparcConfigDirctory = "~/App_Data/DataBase/";
        public const string AntiForgeryTokenSalt = "SOUIDEA__SENPARC";
        public const string WEIXIN_USER_AVATAR_KEY = "SENPARC_"; //将取前8位
        public static readonly long OfficalWeiboUserId = 2513419820;
        public const string DomainName = "https://scf.senparc.com";
        public const string DefaultTemplate = "default";
        public const int SMSSENDWAITSECONDS = 60; //手机验证持续时间
        public const string DEFAULT_AVATAR = "/Content/Images/userinfonopic.png"; //默认头像

        public const string DEFAULT_MEMCACHED_ADDRESS_1 = "192.168.184.91";
        public const int DEFAULT_MEMCACHED_PORT_1 = 11210;

        /// <summary>
        /// WBS格式
        /// </summary>
        public static readonly string WBSFormat = "000";

        /// <summary>
        /// 最大自动发送Email次数
        /// </summary>
        public static readonly int MaxSendEmailTimes = 5;
        /// <summary>
        /// 用户在线不活动过期时间(分钟)
        /// </summary>
        public static readonly int UserOnlineTimeoutMinutes = 10;
        /// <summary>
        /// 最多免验证码尝试登录次数
        /// </summary>
        public static readonly int TryLoginTimes = 1;
        /// <summary>
        /// 最多免验证码尝试登录次数
        /// </summary>
        public static readonly int TryUserLoginTimes = 3;
        /// <summary>
        /// 最大数据库备份文件个数
        /// </summary>
        public static readonly int MaxBackupDatabaseCount = 200;
        /// <summary>
        /// 是否是单元测试
        /// </summary>
        public static readonly bool IsUnitTest = false;

        public static int PageViewCount { get; set; } //网站启动后前台页面浏览量

        //异步线程
        public static Dictionary<string, Thread> AsynThread = new Dictionary<string, Thread>(); //后台运行线程

        /// <summary>
        /// Admin 管理员的 Cookie 登录 Scheme
        /// </summary>
        public readonly static string ScfAdminAuthorizeScheme = "ScfAdminAuthorizeScheme";
        /// <summary>
        /// User 管理员的 Cookie 登录 Scheme
        /// </summary>
        public readonly static string ScfUserAuthorizeScheme = "ScfUserAuthorizeScheme";
    }
}