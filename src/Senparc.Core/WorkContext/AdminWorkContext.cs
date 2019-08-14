using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core
{
    /// <summary>
    /// 管理员区域上下文
    /// </summary>
    public class AdminWorkContext
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int AdminUserId { get; set; }

        /// <summary>
        /// 拥有的角色
        /// </summary>
        public IEnumerable<string> RoleCodes { get; set; }
    }
}
