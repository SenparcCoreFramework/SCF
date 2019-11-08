using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// 数据库数据软删除接口
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否软删除
        /// </summary>
        bool Flag { get; set; }
    }
}
