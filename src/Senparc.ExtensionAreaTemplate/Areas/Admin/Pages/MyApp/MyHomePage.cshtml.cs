using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.ExtensionAreaTemplate.Services;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;

namespace Senparc.ExtensionAreaTemplate.Areas.MyApp.Pages
{
    public class MyHomePage : Senparc.Scf.AreaBase.Admin.AdminXscfModulePageModelBase
    {
        private readonly AreaTemplate_ColorService _areaTemplate_ColorService;
        public MyHomePage(Lazy<XscfModuleService> xscfModuleService) : base(xscfModuleService)
        {

        }

        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnBrightenAsync() { 
            return Task.CompletedTask;

        }
    }
}
