using System;
using System.Collections.Generic;
using Senparc.Scf.Core.Utility;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Senparc.Scf.Mvc.UI
{
    public  static partial class HtmlExtension
    {
        //#region CustomSelect
        ///// <summary>
        ///// 返回由枚举类型生成的自定义样式下拉选项
        ///// </summary>
        //public static HtmlString CustomDropDownListFormEnum(this HtmlHelper htmlHelper, string name, Type enumType, object selectedValue = null, string optionLabel = null, object htmlAttributes = null, bool useDescription = false, bool addBlankOption = false, string blankOptionText = null)
        //{
        //    var selectedList = GetSelectListFromEnum(enumType, selectedValue, useDescription, addBlankOption, blankOptionText);
        //    StringBuilder result = new StringBuilder();
        //    foreach (var item in selectedList)
        //    {
        //        result.AppendFormat("<img src=\"/Images/up_un.gif\" />");
        //    }
        //    return htmlHelper.DropDownList(name, selectedList, optionLabel, htmlAttributes);
        //}
        //#endregion

        #region DropDownList

        /// <summary>
        /// 返回由枚举类型生成的下拉选项
        /// </summary>
        public static IHtmlContent DropDownListFormEnumFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, object selectedValue = null, string optionLabel = null, object htmlAttributes = null, bool useDescription = false, bool addBlankOption = false)
        {
            var selectedList = GetSelectListFromEnum(enumType, selectedValue, useDescription, addBlankOption);
            return htmlHelper.DropDownListFor(expression, selectedList, optionLabel, htmlAttributes);
        }
        /// <summary>
        /// 返回由枚举类型生成的下拉选项
        /// </summary>
        public static IHtmlContent DropDownListFormEnum(this HtmlHelper htmlHelper, string name, Type enumType, object selectedValue = null, string optionLabel = null, object htmlAttributes = null, bool useDescription = false, bool addBlankOption = false, string blankOptionText = null)
        {
            var selectedList = GetSelectListFromEnum(enumType, selectedValue, useDescription, addBlankOption, blankOptionText);
            return htmlHelper.DropDownList(name, selectedList, optionLabel, htmlAttributes);
        }

        public static HtmlString GetDescriptionForEnum(this HtmlHelper htmlHelper, Type enumType, int item)
        {
            return new HtmlString(enumType.GetDescriptionForEnum(item));
        }

        private static SelectList GetSelectListFromEnum(Type enumType, object selectedValue, bool useDescription = false, bool addBlankOption = false, string blankOptionText = null)
        {
            var dic = Extensions.GetDictionaryForEnums(enumType, useDescription, addBlankOption, blankOptionText);
            return GetSelectListFromDictionary(dic, selectedValue);
        }

        private static SelectList GetSelectListFromDictionary(Dictionary<string, string> dic, object selectedValue)
        {
            return new SelectList(dic, "Key", "Value", selectedValue);
        }
        #endregion


        #region Link
        //public static IHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool requireAbsoluteUrl)
        //{
        //    return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        //}

        // more of these 

        //public static IHtmlContent ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        //{
        //    if (requireAbsoluteUrl)
        //    {
        //        HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
        //        RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);

        //        routeData.Values["controller"] = controllerName;
        //        routeData.Values["action"] = actionName;

        //        DomainRoute domainRoute = routeData.Route as DomainRoute;
        //        if (domainRoute != null)
        //        {
        //            DomainData domainData = domainRoute.GetDomainData(new RequestContext(currentContext, routeData), routeData.Values);
        //            return htmlHelper.ActionLink(linkText, actionName, controllerName, domainData.Protocol, domainData.HostName, domainData.Fragment, routeData.Values, null);
        //        }
        //    }
        //    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        //}

        //#region 直接引用代码(仅测试)：http://www.veryhuo.com/a/view/7590.html

        //public static IHtmlContent ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool requireAbsoluteUrl)
        //{
        //    return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        //}

        //// more of these

        //public static IHtmlContent ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        //{
        //    if (requireAbsoluteUrl)
        //    {
        //        HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
        //        RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);

        //        routeData.Values["controller"] = controllerName;
        //        routeData.Values["action"] = actionName;

        //        DomainRoute domainRoute = routeData.Route as DomainRoute;
        //        if (domainRoute != null)
        //        {
        //            DomainData domainData = domainRoute.GetDomainData(new RequestContext(currentContext, routeData), routeData.Values);
        //            return htmlHelper.ActionLink(linkText, actionName, controllerName, domainData.Protocol, domainData.HostName, domainData.Fragment, routeData.Values, null);
        //        }
        //    }
        //    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        //}

        //#endregion

        #endregion

    }
}
