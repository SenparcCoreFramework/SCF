using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    /// <summary>
    /// 角色人员表
    /// </summary>
    public class SysRoleAdminUserInfo : EntityBase<int>
    {
        public SysRoleAdminUserInfo()
        {
            AddTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public SysRoleAdminUserInfo(int accountId, string roleId, string roleCode) : this()
        {
            AccountId = accountId;
            RoleId = roleId;
            RoleCode = roleCode;
        }

        [MaxLength(20)]
        public string RoleCode { get; set; }

        /// <summary>
        /// 管理员Id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [MaxLength(50)]
        public string RoleId { get; set; }
    }
}
