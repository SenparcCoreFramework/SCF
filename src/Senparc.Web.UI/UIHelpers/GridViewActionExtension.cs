using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace System.Web.Mvc
{
    public class GridViewActionItemTemplateModel<T>
    {
        public string Header { get; set; }
        public Action<T, int> ItemTemplate { get; set; }
        public Action<T> Footer { get; set; }
        public object HtmlAttributes { get; set; }
        //public Action<T> EditTemplate { get; set; }


        public GridViewActionItemTemplateModel() { }

        public GridViewActionItemTemplateModel(string header, Action<T, int> itemTemplate, Action<T> footer, object htmlAttributes)
        {
            this.Header = header;
            this.ItemTemplate = itemTemplate;
            this.Footer = footer;
            this.HtmlAttributes = htmlAttributes.ToAttributeList();
        }

        public GridViewActionItemTemplateModel(string header, Action<T, int> itemTemplate, object htmlAttributes)
            : this(header, itemTemplate, null, htmlAttributes)
        { }

        public GridViewActionItemTemplateModel(Action<T, int> itemTemplate)
            : this(null, itemTemplate, null)
        { }

        //public GridViewItemTemplateModel(Func<T, int, string> itemTemplate, Action<T> editTemplate)
        //{
        //    this.ItemTemplate = itemTemplate;
        //    this.EditTemplate = editTemplate;
        //}
    }

    public static class GridViewActionExtension
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
        public static string GridViewAction<T>(this HtmlHelper helper, IEnumerable<T> dataSource,
            object htmlAttributes, string emptyTemplete, bool htmlCodeFormat, params GridViewActionItemTemplateModel<T>[] itemTempletes)
        {
            foreach (var item in dataSource)
            {
                foreach (var temp in itemTempletes)
                {
                    temp.ItemTemplate(item, 0);
                }
            }
            return "ddd";
        }
    }
}
