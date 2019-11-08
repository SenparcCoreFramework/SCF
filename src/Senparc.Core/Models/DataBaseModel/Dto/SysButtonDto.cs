using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class SysButtonDto : BaseDto
    {

        //public bool IsDeleted { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// 菜单id
        /// </summary>
        [MaxLength(50)]
        public string MenuId { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [MaxLength(50)]
        //[Required]
        public string ButtonName { get; set; }

        /// <summary>
        /// 操作标识
        /// </summary>
        [MaxLength(50)]
        public string OpearMark { get; set; }

        /// <summary>
        /// 按钮对应的请求地址
        /// </summary>
        [MaxLength(350)]
        public string Url { get; set; }
    }
}
