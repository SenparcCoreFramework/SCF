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

        public string RoleCode { get; set; }
    }
}
