using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Senparc.Areas.Admin.Filters;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    [AllowAnonymous]
    public class LoginModel : BaseAdminPageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "请输入用户名")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        //绑定参数
        [BindProperty]
        public string ReturnUrl { get; set; }



        private readonly AdminUserInfoService _userInfoService;
        public LoginModel(AdminUserInfoService userInfoService)
        {
            this._userInfoService = userInfoService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            //是否已经登录
            var logined = await base.CheckLoginedAsync(AdminAuthorizeAttribute.AuthenticationScheme);//判断登录

            if (logined)
            {
                if (ReturnUrl.IsNullOrEmpty())
                {
                    return RedirectToPage("/Index");
                }
                return Redirect(ReturnUrl.UrlDecode());
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync(/*[Required]string name,string password*/)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            string errorMsg = null;

            var userInfo = await _userInfoService.GetUserInfo(this.Name);
            if (userInfo == null)
            {
                errorMsg = "账号或密码错误！错误代码：101。";
            }
            else if (_userInfoService.TryLogin(this.Name, this.Password, true) == null)
            {
                errorMsg = "账号或密码错误！错误代码：102。";
            }

            if (!errorMsg.IsNullOrEmpty() || !ModelState.IsValid)
            {
                this.MessagerList = new List<Messager>
                {
                    new Messager(Senparc.Scf.Core.Enums.MessageType.danger, errorMsg)
                };
                return null;
            }

            if (this.ReturnUrl.IsNullOrEmpty())
            {
                return RedirectToPage("/Index");
            }
            return Redirect(this.ReturnUrl.UrlDecode());
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            SenparcTrace.SendCustomLog("管理员退出登录", $"用户名：{base.UserName}");
            await _userInfoService.Logout();
            return RedirectToPage("Index");
        }
    }
}