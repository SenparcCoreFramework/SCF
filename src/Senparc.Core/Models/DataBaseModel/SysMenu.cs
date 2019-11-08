using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class SysMenu : EntityBase<string>
    {

        public SysMenu()
        {
            Id = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
            this.LastUpdateTime = AddTime;
        }

        public SysMenu(SysMenuDto sysMenuDto) : this()
        {
            this.LastUpdateTime = DateTime.Now;
            Icon = sysMenuDto.Icon;
            Sort = sysMenuDto.Sort;
            Visible = sysMenuDto.Visible;
            Url = sysMenuDto.Url;
            ParentId = sysMenuDto.ParentId;
            MenuName = sysMenuDto.MenuName;
        }

        [MaxLength(50)]
        public new string Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string MenuName { get; set; }

        /// <summary>
        /// 父菜单
        /// </summary>
        [MaxLength(50)]
        public string ParentId { get; set; }

        [MaxLength(350)]
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(50)]
        public string Icon { get; set; }

        public void Update(SysMenuDto sysMenuDto)
        {
            this.LastUpdateTime = DateTime.Now;
            Icon = sysMenuDto.Icon;
            Sort = sysMenuDto.Sort;
            Visible = sysMenuDto.Visible;
            Url = sysMenuDto.Url;
            MenuName = sysMenuDto.MenuName;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }
    }
}
