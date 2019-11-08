using System.Text;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace System.Web.Mvc
{
    public static class CustomBindingHelperExtentions
    {
        //public static void UpdateFrom(this object obj, NameValueCollection values, Expression<Func<string, string>> encoder, params string[] keys)
        //{
        //    NameValueCollection encodedValues = new NameValueCollection();
        //    Func<string, string> encodeFunc = encoder.Compile();
        //    foreach (string key in values.Keys)
        //        encodedValues.Add(key, encodeFunc(values[key]));
        //    if (keys.Length > 0)
        //        obj.UpdateFrom(encodedValues, keys);
        //    else
        //        obj.UpdateFrom(encodedValues);
        //}


        public static bool ContainsKey(this NameValueCollection coll, string keyToFind)
        {
            bool bResult = false;
            foreach (string key in coll)
            {
                if (key.ToLower().Trim().Equals(keyToFind.ToLower().Trim()))
                {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }

        /// <summary>
        /// “上移”“下移”箭头
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="listCount"></param>
        /// <param name="currentIndex"></param>
        /// <param name="upFunction"></param>
        /// <param name="upClassEnable"></param>
        /// <param name="upClassDisable"></param>
        /// <param name="downFunction"></param>
        /// <param name="downClassEnable"></param>
        /// <param name="downClassDisable"></param>
        /// <returns></returns>
        public static string UpAndDown(this HtmlHelper helper, int listCount, int currentIndex,
                                    string upFunction, string upClassEnable, string upClassDisable,
                                    string downFunction, string downClassEnable, string downClassDisable)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<span class=\"");
            if (currentIndex == 1)
            {
                result.Append(upClassDisable + "\" >");
                result.AppendFormat("<img src=\"/Images/up_un.gif\" />");//TODO:Put the img in to the css of the up/down SPAN.    ---- BY TNT2    2008.5.30
            }
            else
            {
                result.AppendFormat("{0}\" onclick=\"{1}\" >", upClassEnable, upFunction);
                result.AppendFormat("<img src=\"/Images/up_en.gif\" />");
            }
            result.Append("上移</span>");

            result.Append(" <span class=\"");
            if (currentIndex >= listCount)
            {
                result.Append(downClassDisable + "\" >");
                result.AppendFormat("<img src=\"/Images/down_un.gif\" />");

            }
            else
            {
                result.AppendFormat("{0}\" onclick=\"{1}\" >", downClassEnable, downFunction);
                result.AppendFormat("<img src=\"/Images/down_en.gif\" />");
            }
            result.Append("下移</span>");

            return result.ToString();
        }
    }
}