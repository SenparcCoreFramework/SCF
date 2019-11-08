using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace System.Web.Mvc
{

    public static class OnClickSpanExtension
    {
        /// <summary>
        /// 供Onclick操作的span(样式默认为“onclick”)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="onclickMethod">事件名称（onclick中的所有内容）</param>
        /// <param name="text">文字</param>
        /// <returns></returns>
        public static string OnClickSpan(this HtmlHelper helper, string text, string onclickMethod)
        {
            return OnClickSpan(helper, text, onclickMethod, null, null);
        }

        /// <summary>
        /// 供Onclick操作的span(样式默认为“onclick”)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="onclickMethod">事件名称（onclick中的所有内容）</param>
        /// <param name="text">文字</param>
        /// <returns></returns>
        public static string OnClickSpan(this HtmlHelper helper, string text, string onclickMethod, object htmlAttributes)
        {
            return OnClickSpan(helper, text, onclickMethod, null, htmlAttributes);
        }

        /// <summary>
        /// 供Onclick操作的span
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="cssClass">class样式，如果为空，则使用“onclick”</param>
        /// <param name="onclickMethod">事件名称（onclick中的所有内容）</param>
        /// <param name="text">文字</param>
        /// <returns></returns>
        public static string OnClickSpan(this HtmlHelper helper, string text, string onclickMethod, string cssClass, object htmlAttributes)
        {
            //样式
            cssClass = cssClass ?? "onclick";
            //属性
            string setHash = htmlAttributes.ToAttributeList();
            string attributeList = string.Empty;
            if (setHash != null)
                attributeList = setHash;

            StringBuilder html = new StringBuilder();
            html.AppendFormat("<a href=\"javascript:void(0);\"  onclick=\"{0}\" {1}>{2}</a>", onclickMethod.Replace("\"","\\\'"), attributeList, text);

            return html.ToString();
        }
    }
}
