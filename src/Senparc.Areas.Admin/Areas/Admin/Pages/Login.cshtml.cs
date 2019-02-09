using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Extensions;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    [AllowAnonymous]
    public class LoginModel : BaseAdminPageModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        public bool IsLogined { get; set; }

        //绑定参数
        [BindProperty]
        public string ReturnUrl { get; set; }


        public new async Task<IActionResult> OnGet()
        {
            //是否已经登录
            var authenticate = await HttpContext.AuthenticateAsync(AdminAuthorizeAttribute.AuthenticationScheme);

            if (authenticate.Succeeded)
            {
                if (ReturnUrl.IsNullOrEmpty())
                {
                    return RedirectToPage("/Home/Index");
                }
                return Redirect(ReturnUrl.UrlDecode());
            }

            IsLogined = this.HttpContext.User.Identity.IsAuthenticated;

            return null;
        }
    }
}