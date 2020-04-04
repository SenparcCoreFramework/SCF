using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
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

        private readonly IServiceProvider _serviceProvider;
        private readonly AdminUserInfoService _adminUserInfoService;

        public AdminUserInfo_EditModel(IServiceProvider serviceProvider,AdminUserInfoService adminUserInfoService)
        {
            _serviceProvider = serviceProvider;
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
                    throw new Exception("��Ϣ�����ڣ�");//TODO:��ʱ
                    //return RenderError("��Ϣ�����ڣ�");
                }
             
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            IsEdit = Id > 0;
            this.Validator(AdminUserInfo.UserName, "�û���", "UserName", false)
                .IsFalse(z => this._adminUserInfoService.CheckUserNameExisted(Id, z), "�û����Ѵ��ڣ�", true);
            if (!IsEdit)
            {

                this.Validator(AdminUserInfo.Password, "����", "Password", false).MinLength(6);
            }
            else
            {
                if (!AdminUserInfo.Password.IsNullOrEmpty())
                {
                    this.Validator(AdminUserInfo.Password, "����", "Password", false).MinLength(6);
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

            base.SetMessager(MessageType.success, $"{(IsEdit ? "�޸�" : "����")}�ɹ���");
            return RedirectToPage("./Index");
        }
    }
}