using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Web.Mvc
{
    public static class RepeaterExtension
    {
        public class SingleRepeater : IDisposable
        {
            protected HttpContext _context;

            //bool wroteStartTag = false, wroteEndTag = false;

            string startTag, endTag;//开始、结束标签
            public string itemStartTag, itemEndTag;//单项开始、结束标签
            public StringBuilder body = new StringBuilder();

            public SingleRepeater(HttpContext context, RepeaterMode repeaterMode, object htmlAttributes)
            {
                this._context = context;//目前没有用到，如果直接在这里用context.Response.Write输出则需要。
                switch (repeaterMode)
                {
                    case RepeaterMode.None:
                        startTag = string.Empty;
                        itemStartTag = string.Empty;
                        itemEndTag = string.Empty;
                        endTag = string.Empty;
                        break;
                    case RepeaterMode.Table:
                        startTag = "<table {0}><tbody>";
                        itemStartTag = "<td>";
                        itemEndTag = "</td>";
                        endTag = "</tobdy></table>";
                        break;
                    case RepeaterMode.Div:
                        startTag = "<div {0}>";
                        itemStartTag = "<div>";
                        itemEndTag = "</div>";
                        endTag = "</div>";
                        break;
                    case RepeaterMode.Ul:
                        startTag = "<ul {0}>";
                        itemStartTag = "<li>";
                        itemEndTag = "</li>";
                        endTag = "</ul>";
                        break;
                }

                //属性
                var setHash = htmlAttributes.ToAttributeList();

                string attributeList = string.Empty;
                if (setHash != null)
                    attributeList = setHash;

                startTag = string.Format(startTag, attributeList);

                WriteStartTag();
            }

            public void WriteStartTag()
            {
                body.Append(startTag);
            }

            public void WriteEndTag()
            {
                body.Append(endTag);
            }

            public override string ToString()
            {
                return body.ToString();
            }

            #region IDisposable 成员

            public void Dispose()
            {
                WriteEndTag();
            }

            #endregion
        }

        /// <summary>
        /// Repeater
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <param name="dataSource">数据源</param>
        /// <param name="header">第一个单项循环开始之前的内容</param>
        /// <param name="itemTempletes">单项内容</param>
        /// <param name="separatorTemplate">每次循环分隔内容（Table慎用）</param>
        /// <param name="footer">最后一个单项循环结束之后的内容</param>
        /// <param name="repeaterMode">repeater模式</param>
        /// <param name="colCount">每行烈数（只在repeaterMode为Table时有效）</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static string Repeater<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
            string header, Expression<Func<T, int, string>> itemTempletes, Expression<Func<T, string>> separatorTemplate, string footer,
            RepeaterMode repeaterMode, int colCount, object htmlAttributes)
        {
            if (dataSource == null)
                return "";

            HttpContext context = helper.ViewContext.HttpContext;

            //Dictionary<object, object> data = MvcControlDataBinder.SourceToDictionary(dataSource, "", "");
            SingleRepeater repeater = new SingleRepeater(context, repeaterMode, htmlAttributes);
            using (repeater)
            {
                StringBuilder body = repeater.body;
                Func<T, int, string> itemResult = itemTempletes.Compile();
                Func<T, string> separatorResult = separatorTemplate.Compile();


                //header
                body.Append(header);

                int tableRow = 0;

                /** 单项循环开始 **/

                int dataCount = dataSource.Count();
                int i = 0;
                foreach (var item in dataSource)
                {

                    /* 仅适用Table */
                    if (repeaterMode == RepeaterMode.Table)
                    {
                        if (tableRow % colCount == 0)
                            body.Append("<tr>");//添加行开始标记
                    }

                    body.Append(repeater.itemStartTag);//单项开头

                    switch (repeaterMode)
                    {
                        case RepeaterMode.None:
                        case RepeaterMode.Div:
                        case RepeaterMode.Ul:
                        case RepeaterMode.Table:
                            body.Append(itemResult(item, i).ToString());//主体内容
                            break;
                    }

                    body.Append(repeater.itemEndTag);//单项结尾

                    /* 仅适用Table */
                    if (repeaterMode == RepeaterMode.Table)
                    {
                        tableRow++;
                        if (tableRow % colCount == 0)
                            body.Append("</tr>");//添加行结束标记
                    }

                    //添加分隔符
                    if (i < dataCount - 1)
                    {
                        body.Append(separatorResult(item).ToString());
                    }

                    i++;
                }
                /** 单项循环结束 **/


                //为Table添加结束标记
                if (repeaterMode == RepeaterMode.Table && tableRow % colCount != 0)
                {
                    do
                    {
                        body.Append(repeater.itemStartTag).Append(repeater.itemEndTag);//空白单元格
                        tableRow++;
                    } while (tableRow % colCount != 0);
                    body.Append("</tr>");
                }
                //context.Response.Write(body.ToString());

                //footer
                body.Append(footer);

            }
            return repeater.ToString();
        }

        //public static string Repeater<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
        //    string header, Expression<Func<T, string>> itemTempletes, Expression<Func<T, string>> separatorTemplate, string footer,
        //    RepeaterMode repeaterMode, int colCount, object htmlAttributes)
        //{

        //}

        public enum RepeaterMode
        {
            /// <summary>
            /// 不自动加任何多于标记，等同于foreach。但header和footer仍然有效
            /// </summary>
            None = 0,
            Table,
            Div,
            Ul
        }

        //public class MVCRepeater : Repeater
        //{ 

        //    public Repeater Repeater(this HtmlHelper html,object datasource, string header,string footer,string bodyType)
        //    {
        //        //StringBuilder sb = new StringBuilder();


        //        //return sb.ToString();
        //    }
        //}
    }
}