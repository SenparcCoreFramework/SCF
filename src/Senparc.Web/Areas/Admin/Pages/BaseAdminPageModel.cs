using Senparc.Areas.Admin.Models.VD;
using Senparc.Core.Models.VD;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.VD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Web.Areas.Admin.Pages
{

    public interface IBaseAdminPageModel : IBasePageModel
    {

    }

    public class BaseAdminPageModel : PageModelBase, IBaseAdminPageModel
    {
     
    }
}
