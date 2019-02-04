using Senparc.Scf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Scf.Core.Models
{
    [Serializable]
    public partial class SystemConfig : EntityBase<int>
    {
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string SystemName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string MchId { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string MchKey { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TenPayAppId { get; set; }
    }
}
