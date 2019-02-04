using System.ComponentModel;

namespace Senparc.Scf.Core.Models
{
    public partial class SmsTemplate_Custom : SmsTemplate_Base
    {
        [Description("联系人姓名")]
        public virtual string PersonName { get; set; }

        [Description("联系人头衔")]
        public virtual string PersonTitle { get; set; }
    }
}