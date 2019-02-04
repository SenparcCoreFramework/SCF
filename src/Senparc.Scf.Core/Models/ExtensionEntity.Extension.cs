using Senparc.Scf.Core.Utility;
using System;

namespace Senparc.Scf.Core.Models
{
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

}
