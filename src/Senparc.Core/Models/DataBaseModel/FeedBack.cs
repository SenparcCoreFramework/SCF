using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.Core.Models
{
    [Table(nameof(FeedBack) + "s")]//必须添加前缀，防止全系统中发生冲突（系统表可以不加前缀）
    [Serializable]
    public partial class FeedBack : EntityBase<int>
    {
        public int AccountId { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public Account Account { get; set; }
    }
}