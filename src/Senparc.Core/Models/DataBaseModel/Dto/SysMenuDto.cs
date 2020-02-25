using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class SysMenuDto : BaseDto
    {
      
        /// <summary>
        /// 是否是菜单
        /// </summary>
        public bool IsMenu { get; set; }

        [MaxLength(50)]
        public string Id { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }

        public string ResourceCode { get; set; }

        public SysMenuDto() { }

        public SysMenuDto(bool isMenu, string id, string menuName, string parentId, string url, string icon, int sort, bool visible, string resourceCode)
        {
            IsMenu = isMenu;
            Id = id;
            MenuName = menuName;
            ParentId = parentId;
            Url = url;
            Icon = icon;
            Sort = sort;
            Visible = visible;
            ResourceCode = resourceCode;
        }

    }

    /// <summary>
    /// 菜单树
    /// </summary>
    public class SysMenuTreeItemDto
    {
        public string MenuName { get; set; }

        public string Id { get; set; }

        public bool IsMenu { get; set; }

        public IList<SysMenuTreeItemDto> Children { get; set; }
        public string Icon { get; set; }

        public string Url { get; set; }
    }
}
