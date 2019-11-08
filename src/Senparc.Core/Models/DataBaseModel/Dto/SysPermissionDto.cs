using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class SysPermissionDto : BaseDto
    {
        public string RoleId { get; set; }

        public string PermissionId { get; set; }

        public bool IsMenu { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 资源（按钮）代码
        /// </summary>
        public string ResourceCode { get; set; }
    }
}
