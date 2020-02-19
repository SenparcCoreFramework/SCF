using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
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
        /// 数据库已存的XscfModules
        /// </summary>
        public PagedList<XscfModule> XscfModules { get; set; }
        public List<IXscfRegister> NewXscfRegisters { get; set; }

        private void LoadNewXscfRegisters(PagedList<XscfModule> xscfModules)
        {
            NewXscfRegisters = Senparc.Scf.XscfBase.Register.RegisterList.Where(z => !xscfModules.Exists(m => m.Uid == z.Uid && m.Version == z.Version)).ToList() ?? new List<IXscfRegister>();
        }

        public async Task OnGetAsync()
        {
            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
            LoadNewXscfRegisters(XscfModules);
        }

        /// <summary>
        /// 扫描新模块
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetScanAsync(string uid, string backpage = null)
        {
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("模块不存在！");
            }

            var xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == z.Uid);
            if (xscfRegister == null)
            {
                throw new Exception("模块不存在！");
            }

            var xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == z.Uid && z.Version == xscfRegister.Version);
            if (xscfModule != null)
            {
                throw new Exception("相同版本模块已安装，无需重复安装！");
            }

            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);

            var dto = XscfModules.Select(z => new CreateOrUpdate_XscfModuleDto(z.Name, z.Uid, z.MenuName, z.Version, z.Description, z.UpdateLog, z.AllowRemove, z.State)).ToList();

            //进行模块扫描
            var result = await Senparc.Scf.XscfBase.Register.ScanAndInstall(dto, _xscfModuleService, async (register, installOrUpdate) =>
             {
                 var topMenu = _sysMenuService.GetObject(z => z.MenuName == "扩展模块");
                 var currentMenu = _sysMenuService.GetObject(z => z.ParentId == topMenu.Id && z.MenuName == register.MenuName);//TODO: menu 还需要加一个锁定Uid的扩展属性

                 string menuId = installOrUpdate == InstallOrUpdate.Install ? null : currentMenu?.Id;//如果是Install，必须新建，否则尝试更新
                 var menuDto = new SysMenuDto(true, menuId, register.MenuName, topMenu.Id, $"/Admin/XscfModule/Start/?uid={register.Uid}", "fa fa-bars", 5, true, null);
                 await _sysMenuService.CreateOrUpdateAsync(menuDto).ConfigureAwait(false);
             });
            base.SetMessager(Scf.Core.Enums.MessageType.info, result, true);

            //if (backpage=="Start")
            return RedirectToPage("Start", new { uid = uid });//始终到详情页
            //return RedirectToPage("Index");
        }
    }
}