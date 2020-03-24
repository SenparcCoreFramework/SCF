using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Models.VD;
using Senparc.Scf.AreaBase.Admin;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Core.WorkContext;
using Senparc.Scf.XscfBase;

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

        public override IActionResult RenderError(string message)
        {
            return base.RenderError(message);
        }
    }
}
