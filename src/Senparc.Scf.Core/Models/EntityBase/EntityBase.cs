using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    public class EntityBase : IEntityBase
    {
        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool Flag { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTimeOffset AddTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
    }

    public class EntityBase<TKey> : EntityBase, IEntityBase<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public TKey Id { get; set; }
    }
}
