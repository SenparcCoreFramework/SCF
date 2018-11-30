using System.ComponentModel;

namespace Senparc.Core.Models
{
    public interface ISmsTemplate_Base
    {
        string CompanyName { get; set; }

        string CompanyTel { get; set; }
    }


    public class SmsTemplate_Base : ISmsTemplate_Base
    {
        [Description("本公司名称")]
        public string CompanyName { get; set; }

        [Description("本公司电话")]
        public string CompanyTel { get; set; }
    }
}