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
  
        public MyHomePage(AreaTemplate_ColorService areaTemplate_ColorService, Lazy<XscfModuleService> xscfModuleService)
            : base(xscfModuleService)
        {
            _areaTemplate_ColorService = areaTemplate_ColorService;
        }

        public Task OnGetAsync()
        {
            _areaTemplate_ColorService.BaseData.BaseDB.BaseDataContext.Database.EnsureCreated();
            return Task.CompletedTask;
        }

        public Task OnBrightenAsync() { 
            return Task.CompletedTask;

        }
    }
}
