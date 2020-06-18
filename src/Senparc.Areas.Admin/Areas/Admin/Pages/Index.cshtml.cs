using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Service;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Service;

namespace Senparc.Areas.Admin.Pages
{
    public class IndexModel : BaseAdminPageModel
    {
        public IActionResult OnGet()
        {
            return null;
            //return RedirectToPage("/Home/Index");
        }

        public async Task<IActionResult> OnGetMenuTreeAsync()
        {
            SysMenuService _sysMenuService = HttpContext.RequestServices.GetService<SysMenuService>();
            var sysMenuDtos = await _sysMenuService.GetMenuTreeDtoByCacheAsync();
            return Ok(sysMenuDtos);
        }
    }
}