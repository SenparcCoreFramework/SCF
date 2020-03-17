using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Areas.Admin.SenparcTraceManager;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class SenparcTrace_IndexModel : BaseAdminPageModel
    {
        public List<string> DateList { get; private set; }

        public async Task OnGetAsync()
        {
            DateList = SenparcTraceHelper.GetLogDate();
        }
    }
}
