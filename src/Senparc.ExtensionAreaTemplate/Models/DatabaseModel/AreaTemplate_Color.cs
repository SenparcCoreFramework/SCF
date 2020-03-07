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

        /// <summary>
        /// 颜色码，0-255
        /// </summary>
        public int Bule { get; private set; }

        private Color() { }

        public Color(int red, int green, int bule)
        {
            Red = red;
            Green = green;
            Bule = bule;
        }

        public void Brighten()
        {
            Red = Math.Max(0, Red - 10);
            Green = Math.Max(0, Green - 10);
            Bule = Math.Max(0, Bule - 10);
        }

        public void Darken()
        {
            Red = Math.Max(0, Red + 10);
            Green = Math.Max(0, Green + 10);
            Bule = Math.Max(0, Bule + 10);
        }
    }
}
