using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Senparc.ExtensionAreaTemplate.Services;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto;

namespace Senparc.ExtensionAreaTemplate.Areas.MyApp.Pages
{
    public class MyHomePage : Senparc.Scf.AreaBase.Admin.AdminXscfModulePageModelBase
    {
        public ColorDto ColorDto { get; set; }

        private readonly ColorService _areaTemplate_ColorService;
        private readonly IServiceProvider _serviceProvider;
        public MyHomePage(IServiceProvider serviceProvider, ColorService areaTemplate_ColorService, Lazy<XscfModuleService> xscfModuleService)
            : base(xscfModuleService)
        {
            _areaTemplate_ColorService = areaTemplate_ColorService;
            _serviceProvider = serviceProvider;
        }

        public Task OnGetAsync()
        {
            ColorDto = new ColorDto(_areaTemplate_ColorService.GetObject(z => true, z => z.Id, Scf.Core.Enums.OrderingType.Descending));
            return Task.CompletedTask;
        }

        public Task OnBrightenAsync()
        {
            return Task.CompletedTask;
        }
    }
}
