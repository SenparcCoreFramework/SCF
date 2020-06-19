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
    public class AdminUserInfo_IndexModel : BaseAdminPageModel
    {
        private readonly AdminUserInfoService _adminUserInfoService;
        public PagedList<AdminUserInfo> AdminUserInfoList { get; set; }
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 属性绑定，支持GET
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 排序方法
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string OrderField { get; set; } = "AddTime Desc,Id";

        public AdminUserInfo_IndexModel(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        //[Filters.CustomerResourceFilter("Add")]
        public async Task<IActionResult> OnGetAsync()
        {
            var seh = new SenparcExpressionHelper<AdminUserInfo>();
            var where = seh.BuildWhereExpression();
            var admins = await _adminUserInfoService.GetObjectListAsync(PageIndex, 20, where, OrderField);
            AdminUserInfoList = admins;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminUserInfoName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetListAsync(string adminUserInfoName, int pageIndex, int pageSize)
        {
            var seh = new SenparcExpressionHelper<AdminUserInfo>();
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(adminUserInfoName), _ => _.UserName.Contains(adminUserInfoName));
            var where = seh.BuildWhereExpression();
            var admins = await _adminUserInfoService.GetObjectListAsync(pageIndex, pageSize, where, OrderField);
            //AdminUserInfoList = admins;
            return Ok(new { admins.TotalCount, admins.PageIndex, List = admins.AsEnumerable() });
        }

        public IActionResult OnPostDemo()
        {
            return Ok(null);
        }

        public IActionResult OnPost(int[] ids)
        {
            foreach (var id in ids)
            {
                _adminUserInfoService.DeleteObject(id);
            }

            return RedirectToPage("./Index");
        }


        /// <summary>
        /// Handler=Delete
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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