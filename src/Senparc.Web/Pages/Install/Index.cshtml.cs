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
    public class IndexModel : BasePageModel
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
            var adminUserInfo = _accountInfoService.Init();

            _systemConfigService.Init();

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