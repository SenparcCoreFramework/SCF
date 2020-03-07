using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto
{
    public class AreaTemplate_ColorDto : DtoBase
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
        public int Bule { get; private set; }

        private AreaTemplate_ColorDto() { }

        public AreaTemplate_ColorDto(Color obj)
        {
            Red = obj.Red;
            Green = obj.Green;
            Bule = obj.Bule;
        }
    }
}
