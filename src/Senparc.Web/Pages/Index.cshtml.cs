using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;

namespace Senparc.Web.Pages
{
    public class IndexModel : PageModel
    {
        //public FullSystemConfig FullSystemConfig { get; set; }

        //private FullSystemConfigCache _fullSystemConfigCache;
        //public IndexModel(FullSystemConfigCache fullSystemConfigCache)
        //{
        //    _fullSystemConfigCache = fullSystemConfigCache;
        //}

        public void OnGet()
        {
            //FullSystemConfig = _fullSystemConfigCache.Data;
        }
    }
}
