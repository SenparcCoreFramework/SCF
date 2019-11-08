using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Senparc.Scf.Core.Utility
{
    public static class CommonWebParts
    {
        //#region 转换HTML代码 public static string exHTML(string ntext)
        /////// <summary>
        /////// 转换HTML代码——TNT2
        /////// 已实现：回车,空格
        /////// </summary>
        ////public static string exHTML(string ntext)
        ////{
        ////    ntext = ntext.ToString().Replace(" ", "&nbsp;").Replace(Convert.ToString((char)13), "<br>");
        ////    return ntext;
        ////}

        ///// <summary>
        ///// 转换HTML代码——TNT2
        ///// 已实现：回车,空格
        ///// </summary>
        //public static string ExHTML(this string ntext)
        //{
        //    ntext = ntext.ToString().Replace(" ", "&nbsp;").Replace(Convert.ToString((char)13), "<br />");
        //    return ntext;
        //}

        //public static string ExHtml(string ntext)
        //{
        //    ntext = ntext.ToString().Replace(" ", "&nbsp;").Replace(Convert.ToString((char)13), "<br />");
        //    return ntext;
        //}
        //#endregion

        /// <summary>
        /// 删除所有HTML标记
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelHtml(this string str) {
            if (!string.IsNullOrEmpty(str)) {
                str = Regex.Replace(str, "<[^>]*>", "").Replace("\r\n", "");
            }
            return str;
        }


        /// <summary>
        /// 获取Form值，Handler用
        /// </summary>
        /// <param name="key">Form键</param>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public static string GetFormValue(string key, HttpContext context)
        {
            return context.Request.Form[key].ToString();
        }


        /// <summary>
        /// 获取格式化后的文件名
        /// </summary>
        /// <param name="fileFormat">文件格式（在Config.UpLoadFileFormat中）</param>
        /// <param name="currentFileName">当前文件名（可以包含路径）</param>
        /// <returns></returns>
        public static string GetFormattedUpLoadFileName(string fileFormat, string currentFileName)
        {
            return string.Format(fileFormat, currentFileName, Path.GetExtension(currentFileName));
        }

        #region 货币大小写转换
        /// <summary>
        /// 货币大小写转换
        /// </summary>
        /// <param name="num">货币金额</param>
        /// <returns></returns>
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字   
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字   
            string str3 = ""; //从原num值中取出的值   
            string str4 = ""; //数字的字符串形式   
            string str5 = ""; //人民币大写金额形式   
            int i; //循环变量   
            int j; //num的值乘以100的字符串长度   
            string ch1 = ""; //数字的汉语读法   
            string ch2 = ""; //数字位的汉字读法   
            int nzero = 0; //用来计算连续的零值是几个   
            int temp; //从原num值中取出的值   
            num = Math.Round(Math.Abs(num), 2); //将num取绝对值并四舍五入取2位小数   
            str4 = ((long)(num * 100)).ToString(); //将num乘100并转换成字符串形式   
            j = str4.Length; //找出最高位   
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j); //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分   
            //循环取出每一位需要转换的值   
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1); //取出需转换的某一位的值   
                temp = Convert.ToInt32(str3); //转换为数字   
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时   
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位   
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上   
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;
                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上&#8220;整&#8221;   
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 货币大小写转换
        /// </summary>
        /// <param name="numstr">货币金额</param>
        /// <returns></returns>
        public static string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "非数字形式！";
            }
        }

        #endregion

    }
}
