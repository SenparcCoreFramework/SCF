using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Core.Models.VD;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using Senparc.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [BindProperties()]
    public class LoginModel : BasePageModel/* BaseAdminPageModel*/
    {
        //[BindProperty]
        //[FromBody]
        //[Required(ErrorMessage = "�������û���")]
        //public string Name { get; set; }

        //[BindProperty]
        //[FromBody]
        //[Required(ErrorMessage = "����������")]
        //public string Password { get; set; }

        //�󶨲���
        //[BindProperty(SupportsGet = true)]
        //public string ReturnUrl { get; set; }



        private readonly AdminUserInfoService _userInfoService;
        public LoginModel(AdminUserInfoService userInfoService)
        {
            this._userInfoService = userInfoService;
        }


        public async Task<IActionResult> OnGetAsync(string ReturnUrl)
        {
            await Task.CompletedTask;
            //�Ƿ��Ѿ���¼
            //var logined = await base.CheckLoginedAsync(AdminAuthorizeAttribute.AuthenticationScheme);//�жϵ�¼

            //if (logined)
            //{
            //    if (ReturnUrl.IsNullOrEmpty())
            //    {
            //        return RedirectToPage("/Index");
            //    }
            //    return LocalRedirect(ReturnUrl.UrlDecode());
            //}

            return null;
        }

        //public async Task<IActionResult> OnPostAsync(/*[Required]string name,string password*/)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return null;
        //    }
        //    string errorMsg = null;

        //    var userInfo = await _userInfoService.GetUserInfo(this.Name);
        //    if (userInfo == null)
        //    {
        //        //errorMsg = "�˺Ż�������󣡴�����룺101��";
        //        ModelState.AddModelError(nameof(this.Password), "�˺Ż�������󣡴�����룺101��");
        //    }
        //    else if (_userInfoService.TryLogin(this.Name, this.Password, true) == null)
        //    {
        //        //errorMsg = "�˺Ż�������󣡴�����룺102��";
        //        ModelState.AddModelError(nameof(this.Password), "�˺Ż�������󣡴�����룺102��");
        //    }

        //    if (!errorMsg.IsNullOrEmpty() || !ModelState.IsValid)
        //    {
        //        //this.MessagerList = new List<Messager>
        //        //{
        //        //    new Messager(Senparc.Scf.Core.Enums.MessageType.danger, errorMsg)
        //        //};
        //        return null;
        //    }

        //    if (this.ReturnUrl.IsNullOrEmpty())
        //    {
        //        return RedirectToPage("/Index");
        //    }
        //    return LocalRedirect(this.ReturnUrl.UrlDecode());
        //}

        public async Task<IActionResult> OnPostLoginAsync([FromBody]LoginInDto loginInDto/*[Required]string name,string password*/)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { loginInDto.Name, loginInDto.Password });
            }

            var userInfo = await _userInfoService.GetUserInfo(loginInDto.Name);
            if (userInfo == null)
            {
                //errorMsg = "�˺Ż�������󣡴�����룺101��";
                return Ok("pwd", false, "�˺Ż�������󣡴�����룺101��");
                //ModelState.AddModelError(nameof(this.Password), "�˺Ż�������󣡴�����룺101��");
            }
            else if (_userInfoService.TryLogin(loginInDto.Name, loginInDto.Password, true) == null)
            {
                //errorMsg = "�˺Ż�������󣡴�����룺102��";
                //ModelState.AddModelError(nameof(this.Password), "�˺Ż�������󣡴�����룺102��");
                return Ok("pwd", false, "�˺Ż�������󣡴�����룺102��");
            }

            return Ok(true);
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            SenparcTrace.SendCustomLog("����Ա�˳���¼", $"�û�����{base.UserName}");
            await _userInfoService.LogoutAsync();
            return RedirectToPage(new { area = "Admin" });
        }
    }

    public class LoginInDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}