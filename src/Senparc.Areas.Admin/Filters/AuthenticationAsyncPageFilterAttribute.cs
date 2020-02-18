using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Core;
using Senparc.Core.WorkContext.Provider;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Filters
{

    //https://docs.microsoft.com/zh-cn/aspnet/core/razor-pages/filter?view=aspnetcore-3.1

    public class AuthenticationAsyncPageFilterAttribute : IAsyncPageFilter
    {
        private readonly SysPermissionService _sysPermissionService;
        private readonly IAdminWorkContextProvider _adminWorkContextProvider;
        private readonly SysMenuService _sysMenuService;

        public AuthenticationAsyncPageFilterAttribute(SysPermissionService sysPermissionService, Core.WorkContext.Provider.IAdminWorkContextProvider adminWorkContextProvider, SysMenuService _sysMenuService)
        {
            this._sysPermissionService = sysPermissionService;
            this._adminWorkContextProvider = adminWorkContextProvider;
            this._sysMenuService = _sysMenuService;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            //在调用处理程序方法前，但在模型绑定结束后，进行异步调用。

            //context.ActionDescriptor.FilterDescriptors
            var authenticateResult = await context.HttpContext.AuthenticateAsync(AdminAuthorizeAttribute.AuthenticationScheme);
            if (authenticateResult.Succeeded && !context.Filters.Any(_ => _ is AllowAnonymousFilter))
            {
                BaseAdminPageModel adminPageModel = context.HandlerInstance as BaseAdminPageModel;
                //adminPageModel.SysMenuDtos = await _sysMenuService.GetMenuTreeDtoByCacheAsync();
                adminPageModel.AdminWorkContext = _adminWorkContextProvider.GetAdminWorkContext();

                bool hasPageRoute = context.RouteData.Values.TryGetValue("page", out object page);
                bool hasAreaRoute = context.RouteData.Values.TryGetValue("area", out object area);

                bool hasRight = hasPageRoute && hasAreaRoute;
                if (hasRight)
                {
                    var url = context.HttpContext.Request.Path;/*.GetEncodedPathAndQuery()*/;
                    hasRight = await _sysPermissionService.HasPermissionAsync(url/*string.Concat("/", area, page)*/);
                }

                if (!hasRight && !(adminPageModel is Pages.IndexModel))
                {
                    IActionResult actionResult = new Microsoft.AspNetCore.Mvc.RedirectResult("/Admin/Forbidden");
                    //跳出
                    if (context.HttpContext.Request.Headers.TryGetValue("x-requested-with", out Microsoft.Extensions.Primitives.StringValues strings))
                    {
                        if (strings.Contains("XMLHttpRequest"))
                        {
                            actionResult = new JsonResult(new AjaxReturnModel() { Success = false, Msg = "您没有权限访问" }) { StatusCode = 401 };
                        }
                    }
                    context.Result = actionResult;
                    return;//If an IAsyncPageFilter provides a result value by setting the Result property of PageHandlerExecutingContext to a non-null value, then it cannot call the next filter by invoking PageHandlerExecutionDelegate.
                }
            }
            await next.Invoke();
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            //在选择处理程序方法后，但在模型绑定发生前，进行异步调用。
            return;
        }
    }
}
