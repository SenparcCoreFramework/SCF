using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Senparc.CO2NET.Extensions;

namespace Senparc.Scf.Core.Extensions
{
    public static class StringExtensions
    {
     

        //public static string HtmlDnc(this string InputString)
        //{
        //    string tString = String.Empty;
        //    StringBuilder str = null;
        //    if (!string.IsNullOrEmpty(InputString))
        //    {
        //        tString = InputString;
        //        str = new StringBuilder(tString);
        //        str = str.Replace("&amp;", "&");
        //        str = str.Replace("&lt;", "<");
        //        str = str.Replace("&gt;", ">");
        //        str = str.Replace("　", "  ");
        //        str = str.Replace("&nbsp;", " ");
        //        str = str.Replace("&quot;", "\"\"");
        //        str = str.Replace("<br>", "\n");
        //    }
        //    return str.ToString();
        //}

        //public static string HtmlEnc(this string InputString)
        //{
        //    string tString = String.Empty;
        //    StringBuilder str = null;
        //    if (!string.IsNullOrEmpty(InputString))
        //    {
        //        tString = InputString;
        //        str = new StringBuilder(tString);
        //        str = str.Replace(">", "&gt;");
        //        str = str.Replace("<", "&lt;");
        //        str = str.Replace(" ", " &nbsp;");
        //        str = str.Replace(" ", " &nbsp;");
        //        str = str.Replace("\"", "&quot;");
        //        str = str.Replace("\'", "&#39;");
        //        str = str.Replace("\n", "<br/> ");
        //    }
        //    return str.ToString();
        //}

       
        //public static T ShowWhenNullOrEmpty<T>(this T obj, T defaultConent)
        //{
        //    if (obj == null)
        //    {
        //        return defaultConent;
        //    }
        //    else if (obj is String && obj.ToString() == "")
        //    {
        //        return defaultConent;
        //    }
        //    else
        //    {
        //        return obj;
        //    }
        //}

        ///// <summary>
        ///// 把数据转换为Json格式
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static string ToJson(this object data)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        DataContractJsonSerializer s = new DataContractJsonSerializer(data.GetType());
        //        s.WriteObject(ms, data);
        //        ms.Seek(0, SeekOrigin.Begin);

        //        return Encoding.UTF8.GetString(ms.ToArray());
        //    }
        //}

        ///// <summary>
        ///// 把数据转换为Json格式（使用Newtonsoft.Json.dll）
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static string ToJson(this object data)
        //{
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        //}

        ///// <summary>
        ///// 格式化成Json字符串
        ///// </summary>
        ///// <param name="obj">需要格式化的对象</param>
        ///// <param name="recursionDepth">指定序列化的深度</param>
        ///// <returns>Json字符串</returns>
        //public static string ToJson(this object obj, int recursionDepth)
        //{
        //    //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    //serializer.RecursionLimit = recursionDepth;
        //    //return serializer.Serialize(obj);

        //    return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new Newtonsoft.Json.JsonSerializerSettings()
        //    {
        //        //TODO：设置recursionDepth  COCONET 
        //    });
        //}

        #region 转换HTML代码 public static string exHTML(string ntext)
        ///// <summary>
        ///// 转换HTML代码——TNT2
        ///// 已实现：回车,空格
        ///// </summary>
        //public static string exHTML(string ntext)
        //{
        //    ntext = ntext.ToString().Replace(" ", "&nbsp;").Replace(Convert.ToString((char)13), "<br>");
        //    return ntext;
        //}

        /// <summary>
        /// 转换HTML代码——TNT2
        /// 已实现：回车,空格
        /// </summary>
        public static HtmlString ExHTML(this string ntext)
        {
            if (!string.IsNullOrEmpty(ntext))
            {
                ntext = ntext.ToString().Replace("  ", " &nbsp;").Replace("\n", "<br />");//.Replace(Convert.ToString((char)13), "<br />");
                ntext = Regex.Replace(ntext, @"(?<url>http[s]?://([\w-]+\.)+[\w-]+([/\w-.?=%&;\(\):]*))", "<a href=\"${url}\">${url}</a>", RegexOptions.IgnoreCase);
            }
            return new HtmlString(ntext ?? "");
        }

        /// <summary>
        /// 删除所有HTML标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelHtml(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "<[^>]*>", "").Replace("\r\n", "");
            }
            return str;
        }

        //public static string HtmlEncode(this string str)
        //{
        //    return System.Web.HttpUtility.HtmlEncode(str);
        //}

        //public static string HtmlDecode(this string str)
        //{
        //    return System.Web.HttpUtility.HtmlDecode(str);
        //}

        /// <summary>
        /// 根据布尔值，返回√或×
        /// </summary>
        /// <param name="yesOrNo">true:√,false:×</param>
        /// <returns></returns>
        public static HtmlString YesOrNo(this bool yesOrNo)
        {
            #region 根据布尔值，返回√或×
            if (yesOrNo)
            {
                return new HtmlString("<span class=\"red\">√</span>");
            }
            else
            {
                return new HtmlString("<span class=\"gray\">×</span>");
            }
            #endregion
        }

        /// <summary>
        /// 区字符串固定长度，其余的省略
        /// 
        /// 规则：
        ///  1.如果startIndex大于字符串长度，则自动调整到取最后maxLangth长度。如果此时maxLangth长度比字符串长度还要大，那么startIndex回到0
        ///  2.如果在startIndex基础上，取maxLangth长度大于比字符串长度,那么maxLangth自动取到可能的最大值，即从startIndex一直取到字符串末尾
        ///  3.结果中，字符串只要有削减的地方，都以".."替代
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="maxLangth">最长字符个数</param>
        /// <returns></returns>
        public static string SubString(this string str, int startIndex, int maxLangth)
        {
            if (str == string.Empty || str == null)
            {
                return "";
            }
            else
            {
                string substring = "";

                //调整startIndex
                if (startIndex > str.Length - 1)//如果startIndex大于字符串长度
                {
                    startIndex = (str.Length - maxLangth > 0) ? str.Length - maxLangth : 0;//则自动调整到取最后maxLangth长度。如果此时maxLangth长度比字符串长度还要大，那么startIndex回到0
                }

                //调整maxLangth
                if (startIndex + maxLangth > str.Length)//如果在startIndex基础上，取maxLangth长度大于比字符串长度
                {
                    maxLangth = str.Length - startIndex;//那么maxLangth自动取到可能的最大值，即从startIndex一直取到字符串末尾
                }
                //调整完成

                //加缩略符号
                substring += (startIndex > 0) ? ".." : "";//如果开头削减，以".."替代

                //进行取定长字符串
                substring += str.Substring(startIndex, maxLangth);

                //加缩略符号
                substring += (str.Length - startIndex - maxLangth > 0) ? "..." : "";//如果结尾削减，以".."替代

                return substring;
            }
        }


        /// <summary>
        /// 高亮关键字(红色)
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public static string HighlightKeyword(this string str, string keyword)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (!keyword.IsNullOrEmpty())
            {
                string replaceFormat = "<span class=\"red\">{0}</span>";//替换格式
                str = Regex.Replace(str, string.Format(@"({0})", keyword), string.Format(replaceFormat, "$1"), RegexOptions.IgnoreCase);
            }
            return str;
        }

        /// <summary>
        /// 隐藏IP段
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="hideNum">从后计隐藏几个区段</param>
        /// <returns></returns>
        public static string HideIP(this string ip, int hideNum)
        {
            string[] ipItems = ip.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < hideNum; i++)
            {
                ipItems[3 - i] = "*";
            }
            return string.Join(".", ipItems);
        }

        /// <summary>
        /// 隐藏IP段（只隐藏最后一个IP段）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string HideIP(this string ip)
        {
            return ip.Substring(0, ip.LastIndexOf(".")) + ".*";
        }

        #endregion
    }
}