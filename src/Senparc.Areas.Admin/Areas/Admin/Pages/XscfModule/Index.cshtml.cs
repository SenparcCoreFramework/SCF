using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class XscfModuleIndexModel : BaseAdminPageModel
    {
        private readonly XscfModuleService _xscfModuleService;
        private readonly SysMenuService _sysMenuService;

        public XscfModuleIndexModel(XscfModuleService xscfModuleService, SysMenuService sysMenuService)
        {
            CurrentMenu = "XscfModule";
            this._xscfModuleService = xscfModuleService;
            this._sysMenuService = sysMenuService;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public PagedList<XscfModule> XscfModules { get; set; }



        public async Task OnGetAsync()
        {
            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
        }

        /// <summary>
        /// 扫描新模块
        /// </summary>
        /// <returns></returns>
        public async Task OnGetScanAsync()
        {
            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);
            var dto = XscfModules.Select(z => new CreateOrUpdate_XscfModuleDto(z.Name, z.Uid, z.MenuName, z.Version, z.Description, z.UpdateLog, z.AllowRemove, z.State)).ToList();
            //进行模块扫描
            var result = Senparc.Scf.XscfBase.Register.Scan(dto, _xscfModuleService, async register =>
             {
                 var topMenu = _sysMenuService.GetObject(z => z.MenuName == "扩展模块");
                 var currentMenu = _sysMenuService.GetObject(z => z.ParentId == topMenu.Id && z.MenuName == register.MenuName);
                //TODO: menu 还需要加一个锁定Uid的扩展属性
                if (currentMenu == null)
                 {
                     var menuDto = new SysMenuDto(true, null, register.MenuName, topMenu.Id, $"/Admin/XscfModule/Item/{register.Uid}", "fa fa-bars", 10, true, null);
                     await _sysMenuService.CreateOrUpdateAsync(menuDto).ConfigureAwait(false);
                 }

             });//TODO:异步
            base.SetMessager(Scf.Core.Enums.MessageType.info, result, true);

            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);

        }
    }
}