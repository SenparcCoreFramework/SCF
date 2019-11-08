using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;

namespace Senparc.Areas.Admin.Models.VD
{
    public class Feedback_BaseVD : BaseAdminVD
    {

    }

    public class Feedback_IndexVD : Feedback_BaseVD
    {
        public string Kw { get; set; }
        public PagedList<FeedBack> FeedbackList { get; set; }
    }
}
