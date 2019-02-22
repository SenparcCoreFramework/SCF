using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Validator;
using Senparc.Scf.Service;

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
            bool isEdit = id > 0;
            if (isEdit)
            {
                var userInfo = _adminUserInfoService.GetAdminUserInfo(id);
                if (userInfo == null)
                {
                    throw new Exception("信息不存在！");//TODO:临时
                    //return RenderError("信息不存在！");
                }
                AdminUserInfo.UserName = userInfo.UserName;
                AdminUserInfo.Note = userInfo.Note;
                Id = userInfo.Id;
            }
            IsEdit = isEdit;
            return Page();
        }

        public IActionResult OnPost()
        {
            bool isEdit = Id > 0;
            this.Validator(AdminUserInfo.UserName, "用户名", "UserName", false)
                .IsFalse(z => this._adminUserInfoService.CheckUserNameExisted(Id, z), "用户名已存在！", true);
            if (!isEdit)
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

            AdminUserInfo userInfo = null;
            if (isEdit)
            {
                userInfo = _adminUserInfoService.GetAdminUserInfo(Id);
                if (userInfo == null)
                {
                    return RenderError("信息不存在！");
                }

                userInfo.UpdateObject(AdminUserInfo);
            }
            else
            {
                string userName = AdminUserInfo.UserName;
                string password = AdminUserInfo.Password;
                userInfo = new AdminUserInfo(ref userName, ref password, null, null, AdminUserInfo.Note);
            }

            //await this.TryUpdateModelAsync(userInfo, "", z => z.Note, z => z.UserName);
            this._adminUserInfoService.SaveObject(userInfo);

            base.SetMessager(MessageType.success, $"{(isEdit ? "修改" : "新增")}成功！");
            return RedirectToPage("./Index");
        }
    }
}