using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class SysRoleAdminUserInfoDto : BaseDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public int AdminAccountId { get; set; }

        /// <summary>
        /// 当前用户是否有此角色
        /// </summary>
        public bool HasRole { get; set; }
    }
}
