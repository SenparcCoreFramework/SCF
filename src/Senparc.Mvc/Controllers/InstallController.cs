using Senparc.Service;
using Microsoft.AspNetCore.Mvc;

namespace Senparc.Mvc.Controllers
{
    public class InstallController : BaseController
    {
        private readonly AdminUserInfoService _accountInfoService;
        public InstallController(AdminUserInfoService accountService)
        {
            _accountInfoService = accountService;
        }
        public IActionResult Index()
        {
            _accountInfoService.Init();
            return Content("OK");
        }
    }
}
