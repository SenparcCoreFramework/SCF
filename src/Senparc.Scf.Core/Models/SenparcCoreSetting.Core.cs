using Senparc.CO2NET;
using Senparc.Scf.Core.Enums;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// 全局可调整配置的设置
    /// </summary>
    public partial class SenparcCoreSetting
    {
        /// <summary>
        /// 网站是否开启 Debug 标记
        /// </summary>
        public bool IsDebug { get; set; }
        /// <summary>
        /// 是否是测试站
        /// </summary>
        public bool IsTestSite { get; set; }
        /// <summary>
        /// 对应：AppData/DataBase/SenparcConfig.config 中，所需要使用的数据库连接的 <SenparcConfig> 节点的 Name
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 缓存类型
        /// </summary>

        public CacheType CacheType { get; set; }

        public string MemcachedAddresses { get; set; }
    }
}
