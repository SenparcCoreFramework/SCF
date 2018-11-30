using Senparc.Core.Enums;
using System;

namespace Senparc.Core.Models
{
    public partial class FeedBack : BaseModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Content { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        /// <value></value>
        public Account Account { get; set; }
    }
}