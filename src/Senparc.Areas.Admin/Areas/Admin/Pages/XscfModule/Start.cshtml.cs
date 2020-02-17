using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class XscfModuleStartModel : BaseAdminPageModel
    {
        public IXscfRegister XscfRegister { get; set; }
        public Senparc.Scf.Core.Models.DataBaseModel.XscfModule XscfModule { get; set; }
        public Dictionary<IXscfFunction<IFunctionParameter>, List<FunctionParammeterInfo>> FunctionParammeterInfoCollection { get; set; } = new Dictionary<IXscfFunction<IFunctionParameter>, List<FunctionParammeterInfo>>();

        XscfModuleService _xscfModuleService;
        IServiceProvider _serviceProvider;

        public string Msg { get; set; }

        public XscfModuleStartModel(IServiceProvider serviceProvider,XscfModuleService xscfModuleService)
        {
            _serviceProvider = serviceProvider;
            _xscfModuleService = xscfModuleService;
        }

        public async Task OnGetAsync(string uid)
        {
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("Ä£¿é±àºÅÎ´Ìá¹©£¡");
            }


            XscfModule = _xscfModuleService.GetObject(z => z.Uid == uid);

            if (XscfModule == null)
            {
                throw new Exception("Ä£¿éÎ´×¢²á£¡");
            }

            XscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            if (XscfRegister == null)
            {
                throw new Exception($"Ä£¿é¶ªÊ§»òÎ´¼ÓÔØ£¨{Senparc.Scf.XscfBase.Register.RegisterList.Count}£©£¡");
            }
          
            foreach (var functionType in XscfRegister.Functions)
            {
                var function = _serviceProvider.GetService(functionType);
                Msg = function.GetType().FullName + "," + (function is IXscfFunction<IFunctionParameter>);
            }

            Page();
        }
    }
}
