using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;

namespace Senparc.Scf.Core.Models
{
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