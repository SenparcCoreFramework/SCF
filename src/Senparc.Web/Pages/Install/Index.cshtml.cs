﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models;
using Senparc.Core.Models.VD;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Service;

namespace Senparc.Web.Pages.Install
{
    public class IndexModel : PageModel //不使用基类，因为无法通过已安装程序自动检测
    {
        private readonly XscfModuleServiceExtension _xscfModuleService;
        private readonly AdminUserInfoService _accountInfoService;
        private readonly SystemConfigService _systemConfigService;
        private readonly SysMenuService _sysMenuService;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 管理员用户名
        /// </summary>
        public string AdminUserName { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        public string AdminPassword { get; set; }
        /// <summary>
        /// 需要修改的命名空间
        /// </summary>
        public string Namespace { get; set; }

        public int Step { get; set; }


        public IndexModel(IServiceProvider serviceProvider, XscfModuleServiceExtension xscfModuleService, AdminUserInfoService accountService, SystemConfigService systemConfigService, SysMenuService sysMenuService)
        {
            _xscfModuleService = xscfModuleService;
            _accountInfoService = accountService;
            _sysMenuService = sysMenuService;
            _systemConfigService = systemConfigService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var adminUserInfo = await _accountInfoService.GetObjectAsync(z => true);//检查是否已初始化
                if (adminUserInfo == null)
                {
                    throw new Exception("需要初始化");
                }
            }
            catch (Exception)
            {
                //开始安装系统模块（Service）
                Senparc.Service.Register serviceRegister = new Service.Register();
                await serviceRegister.InstallOrUpdateAsync(_serviceProvider, Scf.Core.Enums.InstallOrUpdate.Install);

                //开始安装系统模块（Admin）
                Senparc.Areas.Admin.Register adminRegister = new Areas.Admin.Register();
                await adminRegister.InstallOrUpdateAsync(_serviceProvider, Scf.Core.Enums.InstallOrUpdate.Install);

                //((SenparcEntities)_accountInfoService.BaseData.BaseDB.BaseDataContext).ResetMigrate();//重置合并状态
                //((SenparcEntities)_accountInfoService.BaseData.BaseDB.BaseDataContext).Migrate();//进行合并
                return Page();
            }

            //base.Response.StatusCode = 404;
            return new StatusCodeResult(404);//已经安装完毕，且存在管理员则不进行安装
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminUserInfo = _accountInfoService.Init(out string userName, out string password);//初始化管理员信息

            if (adminUserInfo == null)
            {
                return new StatusCodeResult(404);
                //base.Response.StatusCode = 404;
                //return;
            }
            else
            {
                Step = 1;
                _systemConfigService.Init();//初始化系统信息
                _sysMenuService.Init();

                IXscfRegister systemRegister = Senparc.Scf.XscfBase.Register.RegisterList.First(z => z.GetType() == typeof(Senparc.Areas.Admin.Register));
                await _xscfModuleService.InstallMenuAsync(systemRegister, Scf.Core.Enums.InstallOrUpdate.Install);//安装菜单

                AdminUserName = userName;
                AdminPassword = password;//这里不可以使用 adminUserInfo.Password，因为此参数已经是加密信息

                return Page();
            }
        }
    }
}