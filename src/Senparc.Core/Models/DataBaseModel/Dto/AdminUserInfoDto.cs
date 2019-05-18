using Senparc.Scf.Core.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models
{
    /// <summary>
    /// AdminUserInfo 创建和更新
    /// </summary>
    public class AdminUserInfoDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Note { get; set; }

        public string RealName { get; set; }

        public string Phone { get; set; }
    }

    public class CreateOrUpdate_AdminUserInfoDto : AdminUserInfoDto
    {
        [Required]
        [StringLength(20)]
        new public string UserName { get; set; }
        new public string Password { get; set; }
    }
}
