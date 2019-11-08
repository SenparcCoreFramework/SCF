namespace Senparc.Scf.Core.Models.WeixinTemplate
{
    public interface IWeixinTemplateBase
    {
        string TemplateId { get; set; }

        string TemplateName { get; set; }
    }


    public class WeixinTemplateBase : IWeixinTemplateBase
    {
        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public WeixinTemplateBase(string templateId, string templateName)
        {
            TemplateId = templateId;
            TemplateName = templateName;
        }
    }
}