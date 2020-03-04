using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Service;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class XscfModuleIndexModel : BaseAdminPageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly XscfModuleService _xscfModuleService;
        private readonly SysMenuService _sysMenuService;

        public XscfModuleIndexModel(IServiceProvider serviceProvider, XscfModuleService xscfModuleService, SysMenuService sysMenuService)
        {
            CurrentMenu = "XscfModule";

            this._serviceProvider = serviceProvider;
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
            //更新菜单缓存
            await _sysMenuService.GetMenuDtoByCacheAsync(true).ConfigureAwait(false);
            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
            LoadNewXscfRegisters(XscfModules);
        }

        /// <summary>
        /// 扫描新模块
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetScanAsync(string uid)
        {
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("模块不存在！");
            }

            var xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            if (xscfRegister == null)
            {
                throw new Exception("模块不存在！");
            }

            var xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == xscfRegister.Uid && z.Version == xscfRegister.Version).ConfigureAwait(false);
            if (xscfModule != null)
            {
                throw new Exception("相同版本模块已安装，无需重复安装！");
            }

            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 999, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);

            var xscfModuleDtos = XscfModules.Select(z => _xscfModuleService.Mapper.Map<CreateOrUpdate_XscfModuleDto>(z)).ToList();

            //进行模块扫描
            InstallOrUpdate? installOrUpdateValue = null;
            var result = await Senparc.Scf.XscfBase.Register.ScanAndInstall(xscfModuleDtos, _serviceProvider, async (register, installOrUpdate) =>
              {
                  installOrUpdateValue = installOrUpdate;
                  var sysMenuService = _serviceProvider.GetService<SysMenuService>();

                  var topMenu = await sysMenuService.GetObjectAsync(z => z.MenuName == "扩展模块").ConfigureAwait(false);
                  var currentMenu = await sysMenuService.GetObjectAsync(z => z.ParentId == topMenu.Id && z.MenuName == register.MenuName).ConfigureAwait(false);
                  SysMenuDto menuDto;

                  if (installOrUpdate == InstallOrUpdate.Update && currentMenu != null)
                  {
                      //更新菜单
                      menuDto = sysMenuService.Mapper.Map<SysMenuDto>(currentMenu);
                      menuDto.MenuName = register.MenuName;//更新菜单名称
                      menuDto.Icon = register.Icon;//更新菜单图标
                  }
                  else
                  {
                      //新建菜单
                      var icon = register.Icon.IsNullOrEmpty() ? "fa fa-bars" : register.Icon;
                      menuDto = new SysMenuDto(true, null, register.MenuName, topMenu.Id, $"/Admin/XscfModule/Start/?uid={register.Uid}", icon, 5, true, null);
                  }

                  var sysMemu = await sysMenuService.CreateOrUpdateAsync(menuDto).ConfigureAwait(false);

                  if (installOrUpdate == InstallOrUpdate.Install)
                  {
                      //更新菜单信息
                      var updateMenuDto = new UpdateMenuId_XscfModuleDto(register.Uid, sysMemu.Id);
                      await _xscfModuleService.UpdateMenuId(updateMenuDto).ConfigureAwait(false);
                  }

              }, uid).ConfigureAwait(false);

            //记录日志
            var installOrUpdateMsg = installOrUpdateValue.HasValue ? (installOrUpdateValue.Value == InstallOrUpdate.Install ? "安装" : "更新") : "失败";
            SenparcTrace.SendCustomLog($"安装或更新模块（{installOrUpdateMsg}）", result.ToString());

            base.SetMessager(Scf.Core.Enums.MessageType.info, result, true);

            //if (backpage=="Start")
            return RedirectToPage("Start", new { uid = uid });//始终到详情页
            //return RedirectToPage("Index");
        }
    }
}