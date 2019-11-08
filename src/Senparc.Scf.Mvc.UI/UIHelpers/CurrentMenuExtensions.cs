using Microsoft.AspNetCore.Mvc.Rendering;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Models.VD;

namespace System.Web.Mvc
{
    public static class CurrentMenuExtensions
    {
        public static string CurrentMenu(this IHtmlHelper htmlHelper, string menuName, string currentClassName = "current active")
        {
            if (htmlHelper.ViewData.Model is IBaseUiVD)
            {
                IBaseUiVD model = htmlHelper.ViewData.Model as IBaseUiVD;
                if (!model.CurrentMenu.IsNullOrEmpty())
                {
                    //int indexOf = model.CurrentMenu.LastIndexOf('.');
                    //string parentMenuMane = model.CurrentMenu.Substring(0, indexOf);
                    var parentMenuNane = model.CurrentMenu.Split('.')[0];
                    if (model.CurrentMenu.StartsWith(menuName, StringComparison.OrdinalIgnoreCase)
                           || parentMenuNane.Equals(menuName, StringComparison.OrdinalIgnoreCase))
                    {
                        return currentClassName;
                        //return "active";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
