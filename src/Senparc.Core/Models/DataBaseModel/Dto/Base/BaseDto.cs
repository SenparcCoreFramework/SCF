using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{
    public class BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(150)]
        public string AdminRemark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(150)]
        public string Remark { get; set; }
    }
}
