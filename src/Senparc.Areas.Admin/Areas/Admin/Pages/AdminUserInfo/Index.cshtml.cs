using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using Senparc.Scf.Utility;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class AdminUserInfo_IndexModel : BaseAdminPageModel
    {
        private readonly AdminUserInfoService _adminUserInfoService;
        public PagedList<AdminUserInfo> AdminUserInfoList { get; set; }

        [BindProperty]
        public int PageIndex { get; set; }

        public AdminUserInfo_IndexModel(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var seh = new SenparcExpressionHelper<AdminUserInfo>();
            var where = seh.BuildWhereExpression();
            var admins = _adminUserInfoService.GetObjectList(PageIndex, 20, where, z => z.Id, OrderingType.Descending);
            AdminUserInfoList = admins;
            return null;
        }

        public IActionResult OnPost(int[] ids)
        {
            foreach (var id in ids)
            {
                _adminUserInfoService.DeleteObject(id);
            }

            return RedirectToPage("./Index");
        }
    }
}