using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Models.VD;
using Senparc.Scf.AreaBase.Admin;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Core.WorkContext;

namespace Senparc.Areas.Admin
{

    public interface IBaseAdminPageModel : IBasePageModel
    {

    }

    //暂时取消权限验证
    //[ServiceFilter(typeof(AuthenticationAsyncPageFilterAttribute))]
    [AdminAuthorize("AdminOnly")]
    public class BaseAdminPageModel : AdminPageModelBase, IBaseAdminPageModel
    {

        /// <summary>
        /// 存储相关用户信息
        /// </summary>
        public AdminWorkContext AdminWorkContext { get; set; }

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
