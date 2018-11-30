using System.ComponentModel;

namespace Senparc.Core.Models
{
    public class SmsTemplate_Custom : SmsTemplate_Base
    {
        [Description("联系人姓名")]
        public string PersonName { get; set; }

        [Description("联系人头衔")]
        public string PersonTitle { get; set; }
    }
}