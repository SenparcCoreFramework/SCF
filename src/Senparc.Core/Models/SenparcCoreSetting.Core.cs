using Senparc.CO2NET;
using Senparc.Core.Enums;

namespace Senparc.Core.Models
{
    /// <summary>
    /// 全局可调整配置的设置
    /// </summary>
    public partial class SenparcCoreSetting
    {
        public bool IsDebug { get; set; }
        public bool IsTestSite { get; set; }

        public CacheType CacheType { get; set; }

        public string MemcachedAddresses { get; set; }
    }
}
