using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class SysRoleDto : BaseDto
    {
        [MaxLength(50)]
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [MaxLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [MaxLength(50)]
        public string RoleCode { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
