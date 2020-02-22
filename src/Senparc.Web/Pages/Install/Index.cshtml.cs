using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models.VD;
using Senparc.Service;

namespace Senparc.Web.Pages.Install
{
    public class IndexModel : PageModel //不使用基类，因为无法通过已安装程序自动检测
    {

        private readonly AdminUserInfoService _accountInfoService;
        private readonly SystemConfigService _systemConfigService;
        private readonly SysMenuService _sysMenuService;

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


        public IndexModel(AdminUserInfoService accountService, SystemConfigService systemConfigService, SysMenuService sysMenuService)
        {
            _accountInfoService = accountService;
            _sysMenuService = sysMenuService;
            _systemConfigService = systemConfigService;
        }

        public IActionResult OnGet()
        {
            var adminUserInfo = _accountInfoService.GetObject(z => true);//检查是否已初始化
            if (adminUserInfo != null)
            {
                return new StatusCodeResult(404);
                //base.Response.StatusCode = 404;
                //return; 
            }
            return Page();
        }

        public IActionResult OnPost()
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

                AdminUserName = userName;
                AdminPassword = password;//这里不可以使用 adminUserInfo.Password，因为此参数已经是加密信息

                return Page();
            }
        }
    }
}