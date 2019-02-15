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
    public interface ICreateUpdate_AdminUserInfoDto : IDtoBase
    {
        [Required]
        [StringLength(20)]
        string UserName { get; set; }
        [Required]
        string Password { get; set; }
    }
}
