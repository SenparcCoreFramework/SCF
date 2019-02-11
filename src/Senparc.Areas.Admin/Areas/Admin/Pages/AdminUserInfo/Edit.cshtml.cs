using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Scf.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class AdminUserInfo_EditModel : BaseAdminPageModel
    {
        /// <summary>
        /// Id
        /// </summary>		
        [BindProperty]
        public int Id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>		
        public string Password { get; set; }
        /// <summary>
        /// 密码盐
        /// </summary>		
        public string PasswordSalt { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>		
        public string RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>		
        public string Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Note { get; set; }
        /// <summary>
        /// 当前登录时间
        /// </summary>		
        public DateTime ThisLoginTime { get; set; }
        /// <summary>
        /// 当前登录IP
        /// </summary>		
        public string ThisLoginIP { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>		
        public DateTime LastLoginTime { get; set; }
        /// <summary>
        /// 上次登录Ip
        /// </summary>		
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>		
        public DateTime AddTime { get; set; }
        public bool IsEdit { get; set; }

        private readonly AdminUserInfoService _adminUserInfoService;


        public AdminUserInfo_EditModel(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        public IActionResult OnGet()
        {
            bool isEdit = Id > 0;
            if (isEdit)
            {
                var userInfo = _adminUserInfoService.GetAdminUserInfo(Id);
                if (userInfo == null)
                {
                    throw new Exception("信息不存在！");//TODO:临时
                    return RenderError("信息不存在！");
                }
                UserName = userInfo.UserName;
                Note = userInfo.Note;
                Id = userInfo.Id;
            }
            IsEdit = isEdit;
            return Page();
        }
    }
}