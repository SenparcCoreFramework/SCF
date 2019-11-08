using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Validator;
using Senparc.Scf.Service;
using Senparc.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class AdminUserInfo_EditModel : BaseAdminPageModel, IValidatorEnvironment
    {
        /// <summary>
        /// Id
        /// </summary>
        [BindProperty]
        public int Id { get; set; }

        public bool IsEdit { get; set; }


        [BindProperty]
        public CreateOrUpdate_AdminUserInfoDto AdminUserInfo { get; set; } = new CreateOrUpdate_AdminUserInfoDto();
        //public CreateUpdate_AdminUserInfoDto AdminUserInfo { get; set; }

        private readonly AdminUserInfoService _adminUserInfoService;


        public AdminUserInfo_EditModel(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        public IActionResult OnGet(int id)
        {
            IsEdit = id > 0;
            if (IsEdit)
            {
                AdminUserInfo = _adminUserInfoService.GetAdminUserInfo(id);
                if (AdminUserInfo == null)
                {
                    throw new Exception("信息不存在！");//TODO:临时
                    //return RenderError("信息不存在！");
                }
             
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            IsEdit = Id > 0;
            this.Validator(AdminUserInfo.UserName, "用户名", "UserName", false)
                .IsFalse(z => this._adminUserInfoService.CheckUserNameExisted(Id, z), "用户名已存在！", true);
            if (!IsEdit)
            {

                this.Validator(AdminUserInfo.Password, "密码", "Password", false).MinLength(6);
            }
            else
            {
                if (!AdminUserInfo.Password.IsNullOrEmpty())
                {
                    this.Validator(AdminUserInfo.Password, "密码", "Password", false).MinLength(6);
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (IsEdit)
            {
                AdminUserInfo.Id = Id;
                _adminUserInfoService.UpdateAdminUserInfo(AdminUserInfo);
            }
            else
            {
                _adminUserInfoService.CreateAdminUserInfo(AdminUserInfo);
            }

            base.SetMessager(MessageType.success, $"{(IsEdit ? "修改" : "新增")}成功！");
            return RedirectToPage("./Index");
        }
    }
}