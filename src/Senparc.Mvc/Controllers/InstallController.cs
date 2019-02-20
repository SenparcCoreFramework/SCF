using Senparc.Service;
using Microsoft.AspNetCore.Mvc;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Service;

namespace Senparc.Mvc.Controllers
{
    public class InstallController : Controller
    {
        private readonly AdminUserInfoService _accountInfoService;
        private readonly SystemConfigService _systemConfigService;
        public InstallController(AdminUserInfoService accountService, SystemConfigService systemConfigService)
        {
            _accountInfoService = accountService;
            _systemConfigService = systemConfigService;
        }
        public IActionResult Index()
        {
            var adminUserInfo = _accountInfoService.Init();

            _systemConfigService.Init();

            if (adminUserInfo == null)
            {
                return new StatusCodeResult(404);
            }

            var vd = new Install_IndexVD()
            {
                AdminUserName = adminUserInfo.UserName,
                AdminPassword = adminUserInfo.Password
            };
            return View(vd);
        }
    }
}
