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
    [ServiceFilter(typeof(AuthenticationResultFilterAttribute))]
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
}
