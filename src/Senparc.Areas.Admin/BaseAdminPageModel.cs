using Microsoft.AspNetCore.Authorization;
using Senparc.Core.Models.VD;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.VD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
