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


        //public virtual void OnGet()
        //{

        //}
    }
}
