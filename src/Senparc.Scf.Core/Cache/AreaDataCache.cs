using System.Collections.Generic;
using Senparc.CO2NET;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;


namespace Senparc.Scf.Core.Cache
{
    /// <summary>
    /// 省数据缓存（来自XML文件中）。不要直接使用此方法，使用Common.AreaData获取。
    /// </summary>
    [AutoDIType(DILifecycleType.Singleton)]
    public class AreaDataCache_Province : BaseCache<List<AreaXML_Provinces>>
    {
        public const string CACHE_KEY = "AreaDataCache:Province";


        public AreaDataCache_Province()
            : base(CACHE_KEY, null)
        {
            base.TimeOut = 9999;
        }

        public override List<AreaXML_Provinces> Update()
        {
            return null;
        }
    }

    /// <summary>
    /// 市数据缓存（来自XML文件中）。不要直接使用此方法，使用Common.AreaData获取。
    /// </summary>
    public class AreaDataCache_City : BaseCache<List<AreaXML_Cities>>
    {
        public const string CACHE_KEY = "AreaDataCache:City";

        public AreaDataCache_City()
            : base(CACHE_KEY, null)
        {
            base.TimeOut = 9999;
        }

        public override List<AreaXML_Cities> Update()
        {
            return null;
        }
    }

    /// <summary>
    /// 区县数据缓存（来自XML文件中）。不要直接使用此方法，使用Common.AreaData获取。
    /// </summary>
    public class AreaDataCache_District : BaseCache<List<AreaXML_Districts>>
    {
        public const string CACHE_KEY = "AreaDataCache:District";

        public AreaDataCache_District()
            : base(CACHE_KEY, null)
        {
            base.TimeOut = 9999;
        }

        public override List<AreaXML_Districts> Update()
        {
            return null;
        }
    }
}
