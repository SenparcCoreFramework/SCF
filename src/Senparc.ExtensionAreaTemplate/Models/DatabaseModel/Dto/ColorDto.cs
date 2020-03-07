using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto
{
    public class ColorDto : DtoBase
    {
        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Red { get; private set; }
        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Green { get; private set; }

        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Blue { get; private set; }

        private ColorDto() { }

        public ColorDto(Color obj)
        {
            Red = obj.Red;
            Green = obj.Green;
            Blue = obj.Blue;
        }

        public ColorDto(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
