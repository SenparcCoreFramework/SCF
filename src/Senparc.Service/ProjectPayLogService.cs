using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Core.Email;
using Senparc.Core.Enums;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Repository;
using Senparc.Log;
using Senparc.Core.Config;

namespace Senparc.Service
{
    public interface IProjectPayLogService : IBaseClientService<ProjectPayLog>
    {
        /// <summary>
        /// 自动生成订单号
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        ProjectPayLog GetBuyOrderNumber(string orderNumber, string[] includes = null);
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="projectDemand"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ProjectPayLog CreateOrder(ProjectDemand projectDemand, string description, int type);
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="projectPayLog"></param>
        void CancelOrder(ProjectPayLog projectPayLog);
    }

    public class ProjectPayLogService : BaseClientService<ProjectPayLog>, IProjectPayLogService
    {
        public ProjectPayLogService(IProjectPayLogRepository projectPayRepo)
            : base(projectPayRepo)
        {
        }

        private string GetAvalibleOrderNumber()
        {
            string orderNumber = null;
            ProjectPayLog payLog = null;

            do
            {
                orderNumber = "C00{0}".With(DateTime.Now.ToString("yyyyMMddHHmmss"));
                payLog = GetBuyOrderNumber(orderNumber);
            } while (payLog != null);

            return orderNumber;
        }

        public ProjectPayLog GetBuyOrderNumber(string orderNumber, string[] includes = null)
        {
            return this.GetObject(z => z.OrderNumber == orderNumber, includes);
        }

        public ProjectPayLog CreateOrder(ProjectDemand projectDemand, string description, int type)
        {
            decimal payMoney = SiteConfig.PROJECTDMANDDEPOSIT;

            var projectPayLog = new ProjectPayLog()
            {
                OrderNumber = null,//Add的时候会自动生成
                AddTime = DateTime.Now,
                CompleteTime = DateTime.Now,
                ProjectDemandId = projectDemand.Id,
                PayMoney = (projectDemand.Price == 0 ? payMoney : projectDemand.Price),
                Description = description,
                Status = (int)ProjectPay_Status.未支付,
                Type = type
            };
            SaveObject(projectPayLog);

            ////发送Email
            //if (account.EmailChecked)
            //{
            //    SendEmail sendEmail = new SendEmail(SendEmailType.OrderCreate);
            //    var sendEmailPara = new SendEmailParameter_OrderCreate(account.Email, account.UserName,
            //                                                            ProjectPayLog.OrderNumber,
            //                                                            ProjectPayLog.PayMoney,
            //                                                            ProjectPayLog.Description);
            //    sendEmail.Send(sendEmailPara, true);
            //}

            return projectPayLog;
        }

        public void CancelOrder(ProjectPayLog projectPayLog)
        {
            projectPayLog.CompleteTime = DateTime.Now;
            projectPayLog.Status = (int)ProjectPay_Status.已取消;
            projectPayLog.Type = (int)ProjectPay_Type.交易关闭;
            SaveObject(projectPayLog);

            ////发送邮件
            //var account = ProjectPayLog.Account;
            //if (account.EmailChecked)
            //{
            //    SendEmail sendEmail = new SendEmail(SendEmailType.OrderCancelled);
            //    var sendEmailPara = new SendEmailParameter_OrderCancelled(account.Email, account.UserName,
            //        ProjectPayLog.OrderNumber,
            //        ProjectPayLog.PayMoney,
            //        ProjectPayLog.Description);
            //    sendEmail.Send(sendEmailPara, true);
            //}
        }

        public override void SaveObject(ProjectPayLog obj)
        {
            var isInsert = base.IsInsert(obj);

            if (isInsert)
            {
                obj.OrderNumber = GetAvalibleOrderNumber();
            }

            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("AccountPayLog{2}：{0}（ID：{1}，当前状态：{3}）", obj.OrderNumber, obj.Id, isInsert ? "新增" : "编辑", (ProjectPay_Status)obj.Status);
        }
        public override void DeleteObject(ProjectPayLog obj)
        {
            LogUtility.WebLogger.InfoFormat("ProjectPay被删除：{0}（ID：{1}）", obj.OrderNumber, obj.Id);
            base.DeleteObject(obj);
        }

    }
}

