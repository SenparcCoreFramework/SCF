using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Core.Models
{
    [Serializable]
    public partial class SystemConfig
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string SystemName
        {
            get;
            set;
        }

        [Column(TypeName = "varchar(100)")]
        public string MchId { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string MchKey { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TenPayAppId { get; set; }
    }
}
