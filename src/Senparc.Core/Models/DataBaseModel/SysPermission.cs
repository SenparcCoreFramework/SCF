using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    public class SysPermission : EntityBase<int>
    {
        public SysPermission()
        {
            AddTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public SysPermission(SysPermissionDto item) : this()
        {
            RoleId = item.RoleId;
            IsMenu = item.IsMenu;
            PermissionId = item.PermissionId;
            RoleCode = item.RoleCode;
        }

        /// <summary>
        /// 角色代码
        /// </summary>
        [MaxLength(20)]
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [MaxLength(50)]
        public string RoleId { get; set; }

        /// <summary>
        /// 是否是菜单
        /// </summary>
        public bool IsMenu { get; set; }

        /// <summary>
        /// 权限Id(菜单或者是按钮)
        /// </summary>
        [MaxLength(50)]
        public string PermissionId { get; set; }
    }
}
