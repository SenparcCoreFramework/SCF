using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;

namespace MyArea.Areas.MyArea.Pages
{
    public class IndexModel : Senparc.Scf.AreaBase.Admin.AdminXscfModulePageModelBase
    {
        public IndexModel(Lazy<XscfModuleService> xscfModuleService) : base(xscfModuleService)
        {

        }

        public void OnGet()
        {
        }
    }
}
