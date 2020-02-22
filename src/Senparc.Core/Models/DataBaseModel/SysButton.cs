using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    /// <summary>
    /// 菜单对应的按钮
    /// </summary>
    public class SysButton : EntityBase<string>
    {
        public SysButton()
        {
            Id = Guid.NewGuid().ToString();
            AddTime = DateTime.Now;
            LastUpdateTime = AddTime;
        }

        public SysButton(SysButtonDto sysButtonDto) : this()
        {
            MenuId = sysButtonDto.MenuId;
            ButtonName = sysButtonDto.ButtonName;
            OpearMark = sysButtonDto.OpearMark;
            Url = sysButtonDto.Url;
        }

        /// <summary>
        /// 菜单id
        /// </summary>
        [MaxLength(50)]
        public string MenuId { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string ButtonName { get; set; }

        /// <summary>
        /// 操作标识
        /// </summary>
        [MaxLength(50)]
        public string OpearMark { get; set; }

        public void Update(SysButtonDto item)
        {
            ButtonName = item.ButtonName;
            OpearMark = item.OpearMark;
            Url = item.Url;
            LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 按钮对应的请求地址
        /// </summary>
        [MaxLength(350)]
        public string Url { get; set; }
    }
}
