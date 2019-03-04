using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Core.Models.VD;
using System.Text;

namespace Senparc.Scf.Mvc.UI
{
    public static class BackendTemplateExtension
    {
        public static ContentBox ContentBox(this HtmlHelper htmlHelper, string title, params object[] tabs)
        {
            return ContentBox(htmlHelper, title, true, tabs);
        }

        public static ContentBox ContentBox(this HtmlHelper htmlHelper, string title, bool showDefaultTabContainer, params object[] tabs)
        {
            StringBuilder tabsCollection = new StringBuilder();
            foreach (var tab in tabs)
            {
                tabsCollection.AppendFormat("<li>{0}</li>", tab);
            }

            var header = ($@"
<div class=""content-box"">
    <!-- Start Content Box -->
    <div class=""content-box-header"">
        <h3>
            {title}</h3>
<ul class=""content-box-tabs {tabsCollection}"">
            {(tabsCollection.Length == 0 ? "hide" : "")}
        </ul>
        <div class=""clear"">
        </div>
    </div>
    <!-- End .content-box-header -->
<div class=""content-box-content"">" +
(showDefaultTabContainer ?
@"
        <div class=""tab-content default-tab"" id=""tab1"">
            <!-- This is the target div. id must match the href of this div's tab -->"
: ""));
            htmlHelper.ViewContext.Writer.Write(header);

            ContentBox contentBox = new ContentBox(htmlHelper.ViewContext, showDefaultTabContainer);
            return contentBox;
        }

        public static HtmlString RenderMessageBox(this IHtmlHelper helper)
        {
            if (!(helper.ViewData.Model is IBaseUiVD model))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();


            if (model.MessagerList != null && model.MessagerList.Count > 0)
            {
                sb.Append("<script>");
                sb.Append("$(function(){");
                foreach (var item in model.MessagerList)
                {
                    sb.Append($"base.alert('{item.MessageType}','{item.MessageText}');");
                }
                sb.Append("});");
                sb.Append("</script>");
            }
            return new HtmlString(sb.ToString());
        }

        public static HtmlString RenderDeveloperMessageBox(this HtmlHelper helper)
        {
            if (!(helper.ViewData.Model is IBaseUiVD model))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (model.MessagerList != null)
            {
                foreach (var item in model.MessagerList)
                {
                    sb.Append(helper.DeveloperMessageBox(item.MessageType, item.MessageText));
                }
            }
            return new HtmlString(sb.ToString());
        }

        public static HtmlString DeveloperMessageBox(this IHtmlHelper helper, MessageType messageType, string messageText)
        {
            string icon = messageType == MessageType.success ? "exclamation" : "remove";
            string format = $@"
<div class=""alert alert-{messageType.ToString()}"">
<i class=""icon-{icon}-sign""></i>
{messageText}
</div>";
            return new HtmlString(format);
        }
    }
}
