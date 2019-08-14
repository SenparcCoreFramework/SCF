using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class RoleEditModel : BaseAdminPageModel
    {
        private readonly SysRoleService _sysRoleService;

        public RoleEditModel(SysRoleService sysRoleService)
        {
            CurrentMenu = "Role";
            _sysRoleService = sysRoleService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public SysRoleDto SysRoleDto { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var entity = await _sysRoleService.GetObjectAsync(_ => _.Id == Id);
                SysRoleDto = _sysRoleService.Mapper.Map<SysRoleDto>(entity);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!string.IsNullOrEmpty(Id))
            //{
            //    var entity = await _sysRoleService.GetObjectAsync(_ => _.Id == Id);
            //    SysRoleDto = _sysRoleService.Mapper.Map<SysRoleDto>(entity);
            //}
            await _sysRoleService.CreateOrUpdateAsync(SysRoleDto);
            return RedirectToPage("./Index");
        }
    }
}