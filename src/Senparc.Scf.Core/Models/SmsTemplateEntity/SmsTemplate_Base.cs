using System.ComponentModel;

namespace Senparc.Scf.Core.Models
{
    public interface ISmsTemplate_Base
    {
        string CompanyName { get; set; }

        string CompanyTel { get; set; }
    }


    public partial class SmsTemplate_Base : ISmsTemplate_Base
    {
        [Description("本公司名称")]
        public virtual string CompanyName { get; set; }

        [Description("本公司电话")]
        public virtual string CompanyTel { get; set; }
    }
}