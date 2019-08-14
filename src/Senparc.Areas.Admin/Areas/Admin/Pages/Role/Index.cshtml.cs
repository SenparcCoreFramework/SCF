using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Models;
using Senparc.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class RoleIndexModel : BaseAdminPageModel
    {
        private readonly SysRoleService _sysRoleService;

        public RoleIndexModel(SysRoleService sysRoleService)
        {
            CurrentMenu = "Role";
            this._sysRoleService = sysRoleService;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public PagedList<SysRole> SysRoles { get; set; }

        public async Task OnGetAsync()
        {
            SysRoles = await _sysRoleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
        }
    }
}