using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Core.Models.VD;
using Senparc.Scf.Service;

namespace Senparc.Web.Pages.Install
{
    public class IndexModel : PageModel //不使用基类，因为无法通过已安装程序自动检测
    {

        private readonly AdminUserInfoService _accountInfoService;
        private readonly SystemConfigService _systemConfigService;

        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }

        public IndexModel(AdminUserInfoService accountService, SystemConfigService systemConfigService)
        {
            _accountInfoService = accountService;
            _systemConfigService = systemConfigService;
        }

        public void OnGet()
        {
            _systemConfigService.Init();//初始化系统信息

            var adminUserInfo = _accountInfoService.Init();//初始化管理员信息


            if (adminUserInfo == null)
            {
                base.Response.StatusCode = 404;
                return;
                //return new StatusCodeResult(404);
            }
            else
            {
                AdminUserName = adminUserInfo.UserName;
                AdminPassword = adminUserInfo.Password;
            }
        }
    }
}