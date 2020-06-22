using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Senparc.Core.Models.VD;
using Senparc.Scf.AreaBase.Admin;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Core.WorkContext;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Senparc.Areas.Admin.Register _xscfRegister;
        public Senparc.Areas.Admin.Register XscfRegister
        {
            get
            {
                _xscfRegister = _xscfRegister ?? new Register();
                return _xscfRegister;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            //context
            if (!context.ModelState.IsValid)
            {
                //全局模型验证
                var state = context.ModelState
                    .Where(_ => _.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    .Select(_ => new { _.Key, Errors = _.Value.Errors.Select(__ => __.ErrorMessage) });
                context.Result = BadRequest(new AjaxReturnModel<object>(state));
            }
            base.OnPageHandlerExecuting(context);
        }

        public override IActionResult RenderError(string message)
        {
            return base.RenderError(message);
        }
    }


    public class ResourceCodeAttribute : Attribute
    {
        public ResourceCodeAttribute(params string[] codes)
        {
            Codes = codes;
        }

        public ResourceCodeAttribute()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string[] Codes { get; set; }
    }

    /// <summary>
    /// 验证是否权限
    /// </summary>
    public class ValidatePermissionFilter : IAsyncPageFilter, IFilterMetadata
    {
        IServiceProvider _serviceProvider;
        public ValidatePermissionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            bool canAccessResource = false;
            ResourceCodeAttribute attributeCodes = context.HandlerMethod.MethodInfo
                .GetCustomAttributes(typeof(ResourceCodeAttribute), false)
                .OfType<ResourceCodeAttribute>()
                .FirstOrDefault();
            IEnumerable<string> resourceCodes = attributeCodes?.Codes.ToList() ?? new List<string>() { "*" };//当前方法的资源Code
            if (resourceCodes.Any(_ => "*".Equals(_)))
            {
                await next();
            }
            else
            {
                canAccessResource = await Task.FromResult(true);//TODO...
                if (canAccessResource)
                {
                    await next();
                }
                else
                {
                    string path = context.HttpContext.Request.Path.Value;
                    IActionResult actionResult = null;
                    if (context.HttpContext.Request.Headers.TryGetValue("x-requested-with", out Microsoft.Extensions.Primitives.StringValues strings))
                    {
                        if (strings.Contains("XMLHttpRequest"))
                        {
                            actionResult = new OkObjectResult(new Scf.Core.AjaxReturnModel<string>(path) { Msg = "您没有权限访问", Success = false }) { StatusCode = (int)System.Net.HttpStatusCode.Forbidden };
                        }
                    }

                    context.Result = actionResult ?? new RedirectResult("/Admin/Forbidden?url=" + System.Web.HttpUtility.UrlEncode(path));
                }
            }
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
