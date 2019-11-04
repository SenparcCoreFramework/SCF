using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Senparc.Core;
using Senparc.Core.WorkContext.Provider;
using Senparc.Scf.Core.Models;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Filters
{
    public class AuthenticationResultFilterAttribute : ResultFilterAttribute
    {
        private readonly SysPermissionService _sysPermissionService;
        private readonly IAdminWorkContextProvider _adminWorkContextProvider;
        private readonly SysMenuService _sysMenuService;

        public AuthenticationResultFilterAttribute(SysPermissionService sysPermissionService, Core.WorkContext.Provider.IAdminWorkContextProvider adminWorkContextProvider, SysMenuService _sysMenuService)
        {
            this._sysPermissionService = sysPermissionService;
            this._adminWorkContextProvider = adminWorkContextProvider;
            this._sysMenuService = _sysMenuService;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //context.ActionDescriptor.FilterDescriptors
            var authenticateResult = await context.HttpContext.AuthenticateAsync(AdminAuthorizeAttribute.AuthenticationScheme);
            if (authenticateResult.Succeeded && !context.Filters.Any(_ => _ is AllowAnonymousFilter))
            {
                BaseAdminPageModel adminPageModel = context.Controller as BaseAdminPageModel;
                //adminPageModel.SysMenuDtos = await _sysMenuService.GetMenuTreeDtoByCacheAsync();
                adminPageModel.AdminWorkContext = _adminWorkContextProvider.GetAdminWorkContext();

                bool hasPageRoute = context.RouteData.Values.TryGetValue("page", out object page);
                bool hasAreaRoute = context.RouteData.Values.TryGetValue("area", out object area);
                bool hasRight = hasPageRoute && hasAreaRoute;
                if (hasRight)
                {
                    hasRight = await _sysPermissionService.HasPermissionAsync(string.Concat("/", area, page));
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
                }
            }
            await base.OnResultExecutionAsync(context, next);
        }
    }
}
