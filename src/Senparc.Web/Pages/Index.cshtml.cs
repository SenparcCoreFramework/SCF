﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Core.Models.VD;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;

namespace Senparc.Web.Pages
{
    public class IndexModel : BasePageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await Task.CompletedTask;
            //判断是否需要自动进入到安装程序
            if (base.FullSystemConfig==null)
            {
                return new RedirectResult("/Install");
            }
            return Page();
        }
    }
}
