using Microsoft.AspNetCore.Mvc;
using Senparc.Areas.Admin.Models.VD;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Mvc.Filter;
using Senparc.Scf.Utility;
using Senparc.Scf.Service;

namespace Senparc.Areas.Admin.Controllers
{
    [MenuFilter("Feedback")]
    public class FeedbackController : BaseAdminController
    {
        private FeedBackService _feedBackService;

        public FeedbackController(FeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        public IActionResult Index(Feedback_IndexVD vd, int pageIndex = 1)
        {
            int pageSize = 20;
            var seh = new SenparcExpressionHelper<FeedBack>();
            seh.ValueCompare
                .AndAlso(!vd.Kw.IsNullOrEmpty(),
                    z => z.Content.Contains(vd.Kw));
            var where = seh.BuildWhereExpression();

            vd.FeedbackList = this._feedBackService.GetObjectList(pageIndex, pageSize, where, z => z.AddTime, OrderingType.Descending, new[] { "Account" });
            return View(vd);
        }
    }
}
