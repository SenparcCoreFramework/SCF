using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Senparc.Areas.Admin.Pages
{
    public class IndexModel : BaseAdminPageModel
    {
        public IActionResult OnGet()
        {
            return null;
            //return RedirectToPage("/Home/Index");
        }
    }
}