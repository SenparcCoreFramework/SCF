using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;

namespace System.Web.Mvc
{
    public class GridViewItemTemplateModel<T>
    {
        public string Header { get; set; }
        public Func<T, int, object> ItemTemplate { get; set; }
        public Func<IEnumerable<T>, string> Footer { get; set; }
        public object HtmlAttributes { get; set; }
        //public Action<T> EditTemplate { get; set; }


        public GridViewItemTemplateModel() { }

        public GridViewItemTemplateModel(string header, Func<T, int, object> itemTemplate, Func<IEnumerable<T>, string> footer, object htmlAttributes)
        {
            this.Header = header;
            this.ItemTemplate = itemTemplate;
            this.Footer = footer;
            this.HtmlAttributes = htmlAttributes.ToAttributeList();
        }

        public GridViewItemTemplateModel(string header, Func<T, int, object> itemTemplate, object htmlAttributes = null)
            : this(header, itemTemplate, null, htmlAttributes)
        { }

        public GridViewItemTemplateModel(Func<T, int, object> itemTemplate, object htmlAttributes = null)
            : this(null, itemTemplate, htmlAttributes)
        { }

        //public GridViewItemTemplateModel(Func<T, int, string> itemTemplate, Action<T> editTemplate)
        //{
        //    this.ItemTemplate = itemTemplate;
        //    this.EditTemplate = editTemplate;
        //}
    }

    public static class GridViewExtension
    {
        /// <summary>
        /// GridView
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <param name="dataSource">数据源</param>
        /// <param name="htmlAttributes">Table的属性</param>
        /// <param name="emptyTemplete">没有数据时显示。没有数据时，Footer不显示</param>
        /// <param name="htmlCodeFormat">HTML代码换行（供调试状态下使用）。如果为false，自动生成的代码将不换行</param>
        /// <param name="itemTempletes">模板数据</param>
        /// <returns></returns>
        public static HtmlString GridView<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
            object htmlAttributes, string emptyTemplete, bool htmlCodeFormat, params GridViewItemTemplateModel<T>[] itemTempletes) where T : class
        {
            string tableFormat = "<table{0}>{1}</table>";

            string headFormat = "<thead>{0}</thead>";
            string headUnitFormat = "<th>{0}</th>";

            string bodyFormat = "<tbody>{0}</tbody>";

            string rowFormat = "<tr>{0}</tr>";
            string UnitFormat = "<td{0}>{1}</td>";//属性，内容

            string footFormat = "<tfoot>{0}</tfoot>";

            string emptyFormat = "{0}";

            StringBuilder table = new StringBuilder();//整个Table

            //thead
            StringBuilder thead = new StringBuilder();//thead
            StringBuilder theadUints = new StringBuilder();//thead内单元格

            //body
            StringBuilder tbody = new StringBuilder();//tbody
            StringBuilder tbodyRows = new StringBuilder();//行

            //tfoot
            StringBuilder tfoot = new StringBuilder();//tfoot
            StringBuilder tfootUints = new StringBuilder();//tfoot内的单元格

            StringBuilder empty = new StringBuilder();


            /*******************/
            /** 构造Table开始 **/
            /*******************/

            //thead
            foreach (var itemTemplete in itemTempletes)
            {
                theadUints.AppendFormat(headUnitFormat, itemTemplete.Header == null ? "" : itemTemplete.Header);
            }
            thead.AppendFormat(headFormat, string.Format(rowFormat, theadUints.ToString()));//head行


            /* tbody 开始 */
            string[] colAttributes = itemTempletes.Select(z => z.HtmlAttributes.ToAttributeList()).ToArray();

            dataSource = dataSource ?? new List<T>();//如果数据为空，则创建一个空的记录，用于获取Count
            int dataSourceCount = dataSource.Count();
            if (dataSourceCount > 0)
            {
                int rowIndex = 0;
                foreach (var data in dataSource)
                {
                    StringBuilder units = new StringBuilder();//tbody一行内的单元格
                    foreach (var itemTemplete in itemTempletes)
                    {
                        string itemResult = itemTemplete.ItemTemplate(data, rowIndex) == null
                            ? ""
                            : itemTemplete.ItemTemplate(data, rowIndex).ToString();//本行模板数据

                        //判断body单元格是否自动绑定
                        if (itemResult.StartsWith("$"))
                        {
                            //自动绑定
                            string bindDataName = itemResult.Replace("$", "");
                            var pro = data.GetType().GetProperty(bindDataName);
                            itemResult = data.GetType().GetProperty(bindDataName).GetValue(data, null).ToString();
                        }

                        //单元格属性

                        units.AppendFormat(UnitFormat, itemTemplete.HtmlAttributes != null ? itemTemplete.HtmlAttributes.ToString() : "", itemResult);//单元格
                    }
                    tbodyRows.AppendFormat(rowFormat, units.ToString());//行
                    rowIndex++;
                }
                tbody.AppendFormat(bodyFormat, tbodyRows.ToString()); //整个tbody
                /* tbody 结束 */


                //tfoot
                foreach (var itemTemplete in itemTempletes)
                {
                    string result = "";

                    if (itemTemplete.Footer != null)
                        result = itemTemplete.Footer(dataSource);

                    tfootUints.AppendFormat(UnitFormat, "", result);//footer没有<fr>？
                }
                tfoot.AppendFormat(footFormat, string.Format(rowFormat, tfootUints.ToString()));//foot行
            }
            else
            {
                empty.AppendFormat(emptyFormat, emptyTemplete);//空数据
            }


            //整合head,body,foot
            StringBuilder tableData = new StringBuilder();
            tableData.Append(thead.ToString()).Append(tbody.ToString()).Append(tfoot.ToString());


            //属性
            string setHash = htmlAttributes.ToAttributeList();
            string attributeList = string.Empty;
            if (setHash != null)
                attributeList = setHash;

            table.AppendFormat(tableFormat, attributeList, tableData.ToString());//整合整个Table，包括属性
            table.Append(empty.ToString());//空数据


            /*******************/
            /** 构造Table结束 **/
            /*******************/

            string tableHtml = string.Empty;

            if (htmlCodeFormat)
                return new HtmlString(table.Replace("><", ">\r\n<").ToString());
            else
                return new HtmlString(table.ToString());

        }


        /// <summary>
        /// 自动绑定GridView
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <param name="dataSource"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string GridView<T>(this HtmlHelper helper, IEnumerable<T> dataSource, object htmlAttributes)
        {
            List<Expression<Func<T, int, string>>> itemTempletes = new List<Expression<Func<T, int, string>>>();
            PropertyInfo[] propertys = typeof(T).GetProperties().Where(z => z.PropertyType.IsValueType).ToArray();//过滤为值类型
            string[] header = propertys.Select(z => z.Name).ToArray();


            //属性
            var setHash = htmlAttributes.ToAttributeList();

            string attributeList = string.Empty;
            if (setHash != null)
                attributeList = setHash;

            StringBuilder table = new StringBuilder();
            table.AppendFormat("<table {0}><thead><tr>", attributeList);
            foreach (var item in header)
            {
                table.AppendFormat("<th>{0}</th>", item);
            }
            table.Append("</tr></thead><tbody>");

            foreach (var item in dataSource)
            {
                var members = item.GetType().GetMembers();
                table.Append("<tr>");
                foreach (var pro in propertys)
                {
                    table.AppendFormat("<td>{0}</td>", pro.GetValue(item, null));
                }
                table.Append("</tr>");
            }
            table.Append("</tbody></table>");


            return table.ToString();
            //return GridView<T>(helper, dataSource, header, null, htmlAttributes, itemTempletes.ToArray());
        }

        //public static string GridView<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
        //   string[] header, string[] footer,
        //   object htmlAttributes, params Expression<Func<T, string>>[] itemTempletes)
        //{
        //    List<Expression<Func<T, int, string>>> items = new List<Expression<Func<T, int, string>>>();

        //    //foreach (var item in itemTempletes)
        //    //{
        //    //    items.Add();
        //    //}

        //    //return GridView<T>(helper, dataSource, header, footer, htmlAttributes, itemTempletes.Select(
        //    //    z => z.Compile().);

        //    return GridView<T>(helper, dataSource, header, footer, htmlAttributes, items.ToArray());

        //}
    }
}
