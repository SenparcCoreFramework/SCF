using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Models;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using Senparc.Scf.Utility;
using Senparc.Service;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Core;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{

    [IgnoreAntiforgeryToken]
    public class AdminUserInfo_IndexModel : BaseAdminPageModel
    {
        private readonly AdminUserInfoService _adminUserInfoService;
        public PagedList<AdminUserInfo> AdminUserInfoList { get; set; }

        /// <summary>
        /// ���԰󶨣�֧��GET
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// ���򷽷�
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string OrderField { get; set; } = "AddTime Desc,Id";

        public AdminUserInfo_IndexModel(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminUserInfoName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Scf.AreaBase.Admin.Filters.CustomerResource("admin-search")]
        public async Task<IActionResult> OnGetListAsync(string adminUserInfoName, int pageIndex, int pageSize)
        {
            var seh = new SenparcExpressionHelper<AdminUserInfo>();
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(adminUserInfoName), _ => _.UserName.Contains(adminUserInfoName));
            var where = seh.BuildWhereExpression();
            var admins = await _adminUserInfoService.GetObjectListAsync(pageIndex, pageSize, where, OrderField);
            return Ok(new { admins.TotalCount, admins.PageIndex, List = admins.AsEnumerable() });
        }

        /// <summary>
        /// Handler=Delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Scf.AreaBase.Admin.Filters.CustomerResource("admin-delete")]
        public IActionResult OnPostDelete([FromBody]int[] ids)
        {
            foreach (var id in ids)
            {
                _adminUserInfoService.DeleteObject(id);
            }
            return Ok(ids.Length);
            //return RedirectToPage("./Index");
        }
    }
}