using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Scf.Service;

namespace Senparc.Web.Pages.Install
{
    public class IndexModel : PageModel
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
            var adminUserInfo = _accountInfoService.Init(out string userName, out string password);

            _systemConfigService.Init();

            if (adminUserInfo == null)
            {
                base.Response.StatusCode = 404;
                return;
                //return new StatusCodeResult(404);
            }
            else
            {
                AdminUserName = userName;
                AdminPassword = password;
            }
        }
    }
}