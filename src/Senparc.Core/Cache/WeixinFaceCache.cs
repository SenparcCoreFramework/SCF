using System.ComponentModel;
using System.Reflection;
using Senparc.Core.Weixin;
using System;
using System.Collections.Generic;
using Senparc.Scf.Core.Cache;

namespace Senparc.Core.Cache
{
    //public interface IWeixinFaceCache : IBaseCache<List<KeyValuePair<string, WeixinFace>>>
    //{
    //}

    /// <summary>
    /// 微信表情缓存
    /// </summary>
    public class WeixinFaceCache : BaseCache<List<KeyValuePair<string, WeixinFace>>>/*, IWeixinFaceCache*/
    {
        public const string CACHE_KEY = "WeixinFaceCache";
        private const int timeout = 1440;

        public WeixinFaceCache()
            : base(CACHE_KEY)
        {
            base.TimeOut = timeout;
        }

        public override List<KeyValuePair<string, WeixinFace>> Update()
        {
            var data = new List<KeyValuePair<string, WeixinFace>>();

            //遍历FaceImage属性
            var enumType = typeof(WeixinFace);
            var names = enumType.GetEnumNames();
            foreach (var name in names)
            {
                var fi = enumType.GetField(name);
                var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if (dna != null)
                {
                    var description = dna.Description;
                    data.Add(new KeyValuePair<string, WeixinFace>(description, (WeixinFace)Enum.Parse(enumType, name)));
                }
            }

            base.SetData(data, base.TimeOut, null);
            return base.Data;
        }
    }
}
