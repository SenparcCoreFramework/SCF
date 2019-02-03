using Senparc.Scf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Core.Models
{
    [Serializable]
    public partial class PointsLog : EntityBase<int>
    {
        public int AccountId { get; set; }
        public int? AccountPayLogId { get; set; }

        public decimal Points { get; set; }

        public decimal BeforePoints { get; set; }

        public decimal AfterPoints { get; set; }

        public string Description { get; set; }

        public virtual AccountPayLog AccountPayLog { get; set; }
        public virtual Account Account { get; set; }
    }
}
