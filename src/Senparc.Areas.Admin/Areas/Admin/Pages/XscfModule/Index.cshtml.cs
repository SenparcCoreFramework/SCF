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
        private readonly XscfModuleServiceExtension _xscfModuleService;
        private readonly SysMenuService _sysMenuService;
        private readonly Lazy<SystemConfigService> _systemConfigService;

        public XscfModuleIndexModel(IServiceProvider serviceProvider, XscfModuleServiceExtension xscfModuleService,
            SysMenuService sysMenuService, Lazy<SystemConfigService> systemConfigService)
        {
            CurrentMenu = "XscfModule";

            this._serviceProvider = serviceProvider;
            this._xscfModuleService = xscfModuleService;
            this._sysMenuService = sysMenuService;
            this._systemConfigService = systemConfigService;
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
            NewXscfRegisters = Senparc.Scf.XscfBase.Register.RegisterList.Where(z => !z.IgnoreInstall && !xscfModules.Exists(m => m.Uid == z.Uid && m.Version == z.Version)).ToList() ?? new List<IXscfRegister>();
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
            var result = await _xscfModuleService.InstallModuleAsync(uid);
            XscfModules = result.Item1;
            base.SetMessager(Scf.Core.Enums.MessageType.info, result.Item2, true);

            //if (backpage=="Start")
            return RedirectToPage("Start", new { uid = uid });//始终到详情页
            //return RedirectToPage("Index");
        }

        /// <summary>
        /// 隐藏“模块管理”功能
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostHideManagerAsync()
        {
            //TODO:使用DTO操作
            var systemConfig = _systemConfigService.Value.GetObject(z => true);
            systemConfig.HideModuleManager = systemConfig.HideModuleManager.HasValue && systemConfig.HideModuleManager.Value == true ? false : true;
            await _systemConfigService.Value.SaveObjectAsync(systemConfig);
            if (systemConfig.HideModuleManager == true)
            {
                return RedirectToPage("../Index");
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        /// <summary>
        /// 隐藏“模块管理”功能 handler=HideManagerAjax
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostHideManagerAjaxAsync()
        {
            //TODO:使用DTO操作
            var systemConfig = _systemConfigService.Value.GetObject(z => true);
            systemConfig.HideModuleManager = systemConfig.HideModuleManager.HasValue && systemConfig.HideModuleManager.Value == true ? false : true;
            await _systemConfigService.Value.SaveObjectAsync(systemConfig);
            if (systemConfig.HideModuleManager == true)
            {
                return RedirectToPage("../Index");
            }
            else
            {
                return RedirectToPage("./Index");
            }
        }

        /// <summary>
        /// 获取已安装模块模块 handler=Mofules
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetMofulesAsync(int pageIndex = 0, int pageSize = 0)
        {
            //更新菜单缓存
            await _sysMenuService.GetMenuDtoByCacheAsync(true).ConfigureAwait(false);
            PagedList<XscfModule> xscfModules = await _xscfModuleService.GetObjectListAsync(pageIndex, pageSize, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
            //xscfModules.FirstOrDefault().
            var xscfRegisterList = XscfRegisterList.Select(_ => new { _.Uid, homeUrl = _.GetAreaHomeUrl() });
            var result = from xscfModule in xscfModules
                         join xscfRegister in xscfRegisterList on xscfModule.Uid equals xscfRegister.Uid
                         into xscfRegister_left
                         from xscfRegister in xscfRegister_left.DefaultIfEmpty()
                         select new
                         {
                             xscfModule,
                             xscfRegister
                         };
            return Ok(result);
        }

        /// <summary>
        /// 获取未安装模块模块 handler=UnMofules
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetUnMofulesAsync()
        {
            var xscfModules = await _xscfModuleService.GetObjectListAsync(0, 0, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
            var newXscfRegisters = Senparc.Scf.XscfBase.Register.RegisterList.Where(z => !z.IgnoreInstall && !xscfModules.Exists(m => m.Uid == z.Uid && m.Version == z.Version)).ToList() ?? new List<IXscfRegister>();
            return Ok(newXscfRegisters);
        }

        /// <summary>
        /// 扫描新模块 handler=ScanAjax
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetScanAjaxAsync(string uid)
        {
            var result = await _xscfModuleService.InstallModuleAsync(uid);
            XscfModules = result.Item1;
            base.SetMessager(Scf.Core.Enums.MessageType.info, result.Item2, true);
            return Ok(uid);
            //return RedirectToPage("Index");
        }
    }
}