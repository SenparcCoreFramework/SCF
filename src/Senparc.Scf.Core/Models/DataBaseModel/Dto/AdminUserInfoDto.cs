using Senparc.Scf.Core.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// AdminUserInfo 创建和更新
    /// </summary>
    public class CreateUpdate_AdminUserInfoDto
    {
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string Note { get; set; }
    }
}
