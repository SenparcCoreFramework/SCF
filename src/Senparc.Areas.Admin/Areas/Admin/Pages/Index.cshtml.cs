using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Senparc.Areas.Admin.Pages
{
    public class Page1Model : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Home/Index");
        }
    }
}