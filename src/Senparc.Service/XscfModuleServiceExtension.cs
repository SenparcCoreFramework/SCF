using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Repository;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Service
{
    public class XscfModuleServiceExtension : XscfModuleService
    {
        private readonly Lazy<SysMenuService> _sysMenuService;
        public XscfModuleServiceExtension(IRepositoryBase<XscfModule> repo, Lazy<SysMenuService> sysMenuService, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            _sysMenuService = sysMenuService;
        }

        /// <summary>
        /// 安装模块
        /// </summary>
        /// <param name="uid">模块 Uid</param>
        /// <returns></returns>
        public async Task<Tuple<PagedList<XscfModule>, string, InstallOrUpdate?>> InstallModuleAsync(string uid)
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

            var xscfModule = await base.GetObjectAsync(z => z.Uid == xscfRegister.Uid && z.Version == xscfRegister.Version).ConfigureAwait(false);
            if (xscfModule != null)
            {
                throw new Exception("相同版本模块已安装，无需重复安装！");
            }

            PagedList<XscfModule> xscfModules = await base.GetObjectListAsync(1, 999, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);

            var xscfModuleDtos = xscfModules.Select(z => base.Mapper.Map<CreateOrUpdate_XscfModuleDto>(z)).ToList();

            //进行模块扫描
            InstallOrUpdate? installOrUpdateValue = null;
            var result = await Senparc.Scf.XscfBase.Register.ScanAndInstall(xscfModuleDtos, _serviceProvider, async (register, installOrUpdate) =>
            {
                installOrUpdateValue = installOrUpdate;
                //底层系统模块此时还没有设置好初始化的菜单信息，不能设置菜单
                if (register.Uid != Senparc.Scf.Core.Config.SiteConfig.SYSTEM_XSCF_MODULE_SERVICE_UID)
                {
                    await InstallMenuAsync(register, installOrUpdate);
                }
            }, uid).ConfigureAwait(false);

            //记录日志
            var installOrUpdateMsg = installOrUpdateValue.HasValue ? (installOrUpdateValue.Value == InstallOrUpdate.Install ? "安装" : "更新") : "失败";
            SenparcTrace.SendCustomLog($"安装或更新模块（{installOrUpdateMsg}）", result.ToString());
            return new Tuple<PagedList<XscfModule>, string, InstallOrUpdate?>(xscfModules, result, installOrUpdateValue);
        }

        /// <summary>
        /// 安装模块之后，安装菜单
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public async Task InstallMenuAsync(IXscfRegister register, InstallOrUpdate installOrUpdate)
        {
            var topMenu = await _sysMenuService.Value.GetObjectAsync(z => z.MenuName == "扩展模块").ConfigureAwait(false);
            var currentMenu = await _sysMenuService.Value.GetObjectAsync(z => z.ParentId == topMenu.Id && z.MenuName == register.MenuName).ConfigureAwait(false);
            SysMenuDto menuDto;

            if (installOrUpdate == InstallOrUpdate.Update && currentMenu != null)
            {
                //更新菜单
                menuDto = _sysMenuService.Value.Mapper.Map<SysMenuDto>(currentMenu);
                menuDto.MenuName = register.MenuName;//更新菜单名称
                menuDto.Icon = register.Icon;//更新菜单图标
            }
            else
            {
                //新建菜单
                var icon = register.Icon.IsNullOrEmpty() ? "fa fa-bars" : register.Icon;
                var order = 20;
                switch (register.Uid)
                {
                    case SiteConfig.SYSTEM_XSCF_MODULE_SERVICE_UID:
                        order = 160;
                        break;
                    case SiteConfig.SYSTEM_XSCF_MODULE_AREAS_ADMIN_UID:
                        order = 150;
                        break;
                    default:
                        break;
                }
                menuDto = new SysMenuDto(true, null, register.MenuName, topMenu.Id, $"/Admin/XscfModule/Start/?uid={register.Uid}", icon, order, true, null);
            }

            var sysMemu = await _sysMenuService.Value.CreateOrUpdateAsync(menuDto).ConfigureAwait(false);

            if (installOrUpdate == InstallOrUpdate.Install)
            {
                //更新菜单信息
                var updateMenuDto = new UpdateMenuId_XscfModuleDto(register.Uid, sysMemu.Id);
                await base.UpdateMenuId(updateMenuDto).ConfigureAwait(false);
            }
        }
    }
}
