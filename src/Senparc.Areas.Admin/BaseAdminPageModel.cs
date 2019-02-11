using Microsoft.AspNetCore.Mvc;
using Senparc.Areas.Admin.Filters;
using Senparc.Core.Models.VD;
using Senparc.Scf.Core.Models.VD;

namespace Senparc.Areas.Admin
{

    public interface IBaseAdminPageModel : IBasePageModel
    {

    }

    [AdminAuthorize("AdminOnly")]
    public class BaseAdminPageModel : PageModelBase, IBaseAdminPageModel
    {

        public virtual IActionResult RenderError(string message)
        {
            //保留原有的controller和action信息
            //ViewData["FakeControllerName"] = RouteData.Values["controller"] as string;
            //ViewData["FakeActionName"] = RouteData.Values["action"] as string;

            return Page();//TODO：设定一个特定的错误页面

            //return View("Error", new Error_ExceptionVD
            //{
            //    //HandleErrorInfo = new HandleErrorInfo(new Exception(message), Url.RequestContext.RouteData.GetRequiredString("controller"), Url.RequestContext.RouteData.GetRequiredString("action"))
            //});
        }
    }
}
