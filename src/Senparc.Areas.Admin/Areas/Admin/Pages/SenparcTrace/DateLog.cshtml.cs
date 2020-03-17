using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Areas.Admin.SenparcTraceManager;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class SenparcTrace_DateLogModel : BaseAdminPageModel
    {
        public string Date { get; private set; }
        public List<SenparcTraceItem> WeixinTraceItemList { get; private set; }

        public async Task OnGetAsync(string date)
        {
            Date = date;
            WeixinTraceItemList = SenparcTraceHelper.GetAllLogs(date);
        }
    }
}
