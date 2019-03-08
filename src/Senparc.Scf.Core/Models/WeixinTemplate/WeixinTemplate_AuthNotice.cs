using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Senparc.Scf.Core.Models.WeixinTemplate
{
    /// <summary>
    /// 审核结果通知
    /// </summary>
    public class WeixinTemplate_AppAuditNotice : WeixinTemplateBase
    {
        /*
        {{first.DATA}}
        审核事项：{{keyword1.DATA}}
        审核状态：{{keyword2.DATA}}
        审核时间：{{keyword3.DATA}}
        {{remark.DATA}}
        * 
        你的审核请求已经处理。
        审核事项：机构申请注册
        审核状态：通过
        审核时间：2017年2月5日18:17
        点击查看详情
        */
        public TemplateDataItem first { get; set; }

        public TemplateDataItem keyword1 { get; set; }

        public TemplateDataItem keyword2 { get; set; }

        public TemplateDataItem keyword3 { get; set; }

        public TemplateDataItem remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_first"></param>
        /// <param name="_keyword1">认证详情</param>
        /// <param name="_keyword2">认证结果</param>
        /// <param name="_remark"></param>
        public WeixinTemplate_AppAuditNotice(string _first, string _keyword1, string _keyword2, string _keyword3,
            string _remark) : base("_1_gRx85XBc7OGY4dw9D-fyW4XEeu6Q5URn_ikKH2UD", "审核结果通知")
        {
            first = new TemplateDataItem(_first);
            keyword1 = new TemplateDataItem(_keyword1);
            keyword2 = new TemplateDataItem(_keyword2);
            keyword3 = new TemplateDataItem(_keyword3);
            remark = new TemplateDataItem(_remark);
        }
    }
}