using Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
using Senparc.Scf.Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senparc.ExtensionAreaTemplate
{
    [Serializable]
    [Table(Register.DATABASE_PREFIX + "Color")]
    public class Color : EntityBase<int>
    {
        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Red { get; private set; }
        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Green { get; private set; }

        private int blue;

        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Blue { get; set; }

        private Color() { }

        public Color(int red, int green, int blue)
        {
            if (red == -1 && green == -1 && blue == -1)
            {
                //随机产生颜色代码
                var radom = new Random(SystemTime.Now.Second);
                Func<int> getRadomColorCode = () => radom.Next(0, 255);
                red = getRadomColorCode();
                green = getRadomColorCode();
                blue = getRadomColorCode();
            }

            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color(ColorDto colorDto)
        {
            Red = colorDto.Red;
            Green = colorDto.Green;
            Blue = colorDto.Blue;
        }

        public void Brighten()
        {
            Red = Math.Max(0, Red - 10);
            Green = Math.Max(0, Green - 10);
            Blue = Math.Max(0, Blue - 10);
        }

        public void Darken()
        {
            Red = Math.Min(255, Red + 10);
            Green = Math.Min(255, Green + 10);
            Blue = Math.Min(255, Blue + 10);
        }
    }
}
