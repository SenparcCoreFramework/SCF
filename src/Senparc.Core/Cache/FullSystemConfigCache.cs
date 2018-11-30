using System.Linq;
using Senparc.Core.Models;

namespace Senparc.Core.Cache
{
    //public interface IFullSystemConfigCache : IBaseCache<FullSystemConfig>
    //{

    //}

    public class FullSystemConfigCache : BaseCache<FullSystemConfig>/*, IFullSystemConfigCache*/
    {
        public const string CACHE_KEY = "FullSystemConfigCache";

        public FullSystemConfigCache(ISqlClientFinanceData db)
            : base(CACHE_KEY, db)
        {
            base.TimeOut = 1440;
        }

        public override FullSystemConfig Update()
        {
            var systemConfig = base._db.DataContext.SystemConfigs.FirstOrDefault();
            FullSystemConfig fullSystemConfig = null;
            if (systemConfig != null)
            {
                fullSystemConfig = FullSystemConfig.CreateEntity<FullSystemConfig>(systemConfig);
            }
            base.SetData(fullSystemConfig, base.TimeOut, null);
            return base.Data;
        }
    }
}
