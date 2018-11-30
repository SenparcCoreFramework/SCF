using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Senparc.Core.Config;
using Senparc.Core.Email;
using Senparc.Core.Enums;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Core.Utility;
using Senparc.Repository;
using Senparc.Log;
using Senparc.Utility;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.GoogleMap;
using Senparc.Weixin.MP.Helpers;
using StructureMap;

namespace Senparc.Service
{
    public interface IProjectDemandService : IBaseClientService<ProjectDemand>
    {
        /// <summary>
        /// 用户选择竞标
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="projectDemand"></param>
        void Choose(int developerId, ProjectDemand projectDemand);
        /// <summary>
        /// 发布任务则用户ProjectDemandCount加1
        /// </summary>
        /// <param name="accountId"></param>
        void AddDemandCount(int accountId);
        /// <summary>
        /// 撤销任务则用户ProjectDemandCount减1
        /// </summary>
        /// <param name="accountId"></param>
        void ReduceDemandCount(int accountId);
        /// <summary>
        /// 发布任务则用户ProjectDemandCount加1
        /// </summary>
        /// <param name="accountId"></param>
        void AddSuccessDemandCount(int accountId);
        /// <summary>
        /// 撤销任务
        /// </summary>
        /// <param name="projectDemand"></param>
        void Revoke(ProjectDemand projectDemand);
        /// <summary>
        /// 推动步骤
        /// </summary>
        /// <param name="projectDemand"></param>
        /// <param name="step">当前任务所在的步骤</param>
        void Step(ProjectDemand projectDemand, int step);
        /// <summary>
        /// 过期任务自动失效
        /// </summary>
        /// <param name="id">AccountId</param>
        void Expired(int? id = null);
        /// <summary>
        /// 充值返回信息处理
        /// </summary>
        /// <param name="projectPayLog"></param>
        /// <param name="tradeNo"></param>
        /// <returns>ProjectDemand</returns>
        ProjectDemand RecordAlipayTrade(ProjectPayLog projectPayLog, string tradeNo);
        /// <summary>
        /// 撤销任务申请退还押金
        /// </summary>
        /// <param name="projectDemand"></param>
        void BackMortgage(ProjectDemand projectDemand);
        /// <summary>
        /// 同意付款给开发者
        /// </summary>
        /// <param name="projectDemand"></param>
        void AgreePay(ProjectDemand projectDemand);
        /// <summary>
        /// 管理员将雇主托管的赏金支付给开发者后执行
        /// </summary>
        /// <param name="projectDemand"></param>
        /// <param name="account"></param>
        void SuccessDemand(ProjectDemand projectDemand, Account account);
        /// <summary>
        /// 添加评论时间
        /// </summary>
        /// <param name="projectDemandId"></param>
        void CommintTime(int projectDemandId);
        /// <summary>
        /// 发布任务扣除押金（用户积分足够时）
        /// </summary>
        /// <param name="projectDemand"></param>
        /// <param name="account"></param>
        void DeductPoints(ProjectDemand projectDemand, Account account);
        /// <summary>
        /// 获取热门任务
        /// </summary>
        /// <returns></returns>
        List<ProjectDemand> GetHotProjectDemandList();

    }

    public class ProjectDemandService : BaseClientService<ProjectDemand>, IProjectDemandService
    {
        private IProjectBiddingService _projectBiddingService;
        private AccountService _accountService;
        private IProjectPayLogService _projectPayLogService;
        private IPointsLogService _pointsLogService;
        private IDeveloperPointsLogService _developerPointsLogService;
        private IMessageBoxService _messageBoxService;
        public ProjectDemandService(IProjectDemandRepository projectDemandRepo, IMessageBoxService messageBoxService, IDeveloperPointsLogService developerPointsLogService, IPointsLogService pointsLogService, IProjectBiddingService projectBiddingService, AccountService accountService, IProjectPayLogService projectPayLogService)
            : base(projectDemandRepo)
        {
            this._projectBiddingService = projectBiddingService;
            this._accountService = accountService;
            this._projectPayLogService = projectPayLogService;
            this._pointsLogService = pointsLogService;
            this._developerPointsLogService = developerPointsLogService;
            this._messageBoxService = messageBoxService;
        }

        public void Choose(int developerId, ProjectDemand projectDemand)
        {
            BiddingStateChange(projectDemand, (int)ProjectBidding_State.未中标);

            var projectBidding = _projectBiddingService.GetObject(z => z.ProjectDemandId == projectDemand.Id && z.DeveloperId == developerId);//中标 状态改为已中标
            projectBidding.State = (int)ProjectBidding_State.已中标;
            _projectBiddingService.SaveObject(projectBidding);

            projectDemand.State = (int)ProjectDemand_State.进行中;
            projectDemand.WinBiddingId = projectBidding.Id;
            projectDemand.DeveloperId = developerId;
            SaveObject(projectDemand);
        }

        public void AddDemandCount(int accountId)
        {
            var account = _accountService.GetObject(accountId);
            account.ProjectDemandCount++;
            _accountService.SaveObject(account);
        }

        public void ReduceDemandCount(int accountId)
        {
            var account = _accountService.GetObject(accountId);
            account.ProjectDemandCount--;
            _accountService.SaveObject(account);
        }

        public void AddSuccessDemandCount(int accountId)
        {
            var account = _accountService.GetObject(accountId);
            account.SuccessProjectDemandCount++;
            _accountService.SaveObject(account);
        }

        /// <summary>
        /// 统一修改所有竞标该任务的状态
        /// </summary>
        /// <param name="projectDemand">任务</param>
        /// <param name="projectBiddingState">所有竞标要修改成的状态</param>
        protected void BiddingStateChange(ProjectDemand projectDemand, int projectBiddingState)
        {
            var projectBiddingList = _projectBiddingService.GetFullList(z => z.ProjectDemandId == projectDemand.Id, z => z.Id,
                                                                        OrderingType.Descending);
            for (int i = 0; i < projectBiddingList.Count; i++)
            {
                projectBiddingList[i].State = projectBiddingState;
                _projectBiddingService.SaveObject(projectBiddingList[i]);
            }
        }

        public void Revoke(ProjectDemand projectDemand)
        {
            BiddingStateChange(projectDemand, (int) ProjectBidding_State.竞标撤销);
            projectDemand.State = (int)ProjectDemand_State.撤销;
            SaveObject(projectDemand);
        }

        public void BackMortgage(ProjectDemand projectDemand)
        {
            projectDemand.FinishTime = DateTime.Now;
            SaveObject(projectDemand);

            var payMoney = projectDemand.Price == 0 ? SiteConfig.PROJECTDMANDDEPOSIT : projectDemand.Price;
            var account = projectDemand.Account;
            var projectPayLog = _projectPayLogService.GetObject(z => z.ProjectDemandId == projectDemand.Id);
            if (projectPayLog != null)
            {
                _pointsLogService.ChangePoint(account, projectPayLog.PayMoney, "{0}任务押金返还".With(projectDemand.Title));
                //将支付记录修改成已退还
                projectPayLog.Type = (int) ProjectPay_Type.已退还;
                projectPayLog.CompleteTime = DateTime.Now;
                _projectPayLogService.SaveObject(projectPayLog);
            }
            else
            {
                _pointsLogService.ChangePoint(account, payMoney, "{0}任务押金返还".With(projectDemand.Title));
            }
        }

        public void Step(ProjectDemand projectDemand, int step)
        {
            if (projectDemand.Step == step)//避免查看前几步时改变步骤
            {
                projectDemand.Step++;
                SaveObject(projectDemand);
            }
        }

        public void Expired(int? id = null)
        {
            List<ProjectDemand> projectDemandList = null;

            if (id.HasValue)
            {
                projectDemandList = GetFullList(z => z.AccountId == id, z => z.Id, OrderingType.Descending);
            }
            else
            {
                projectDemandList = GetFullList(z => true, z => z.Id, OrderingType.Descending);
            }
            for (int i = 0; i < projectDemandList.Count; i++)//过期任务自动失效
            {
                if (projectDemandList[i].EndTime < DateTime.Now)
                {
                    projectDemandList[i].State = (int)ProjectDemand_State.撤销;
                    SaveObject(projectDemandList[i]);
                }
            }
        }

        public ProjectDemand RecordAlipayTrade(ProjectPayLog projectPayLog, string tradeNo)
        {
            var projectPayLogService = ObjectFactory.GetInstance<IProjectPayLogService>();
            var projectDemand = projectPayLog.ProjectDemand;
            var account = projectDemand.Account;

            projectPayLog.CompleteTime = DateTime.Now;
            projectPayLog.TradeNumber = tradeNo;
            projectPayLog.Status = (int)ProjectPay_Status.已支付;
            projectPayLogService.SaveObject(projectPayLog);
            JudgePay(projectDemand);
            ////发送Email
            //if (account.EmailChecked)
            //{
            //    SendEmail sendEmail = new SendEmail(SendEmailType.OrderPaySuccess);
            //    var sendEmailPara = new SendEmailParameter_OrderPaySuccess(account.Email, account.UserName,
            //        projectPayLog.OrderNumber,
            //        projectPayLog.PayMoney,
            //        projectPayLog.Description);
            //    sendEmail.Send(sendEmailPara, true);

            //    //发送站内信
            //    _messageBoxService.SendMessage(sendEmail.SentSubject, sendEmail.SentBody, account.Id);
            //}
            return projectDemand;
        }

        private void JudgePay(ProjectDemand projectDemand)//判断任务是否支付押金
        {
            projectDemand.Mortgaged = true;
            projectDemand.State = (int)ProjectDemand_State.竞标中;
            SaveObject(projectDemand);
        }

        public void AgreePay(ProjectDemand projectDemand)
        {
            var projectBidding = projectDemand.ProjectBidding;
            var developer = projectBidding.Developer;
            var account = projectDemand.Account;
            decimal payMoney = 0;

            if (!projectDemand.HasPaid)
            {
                if (projectDemand.Price > 0)//雇主标价
                {
                    _developerPointsLogService.AddPoint(developer, projectDemand.Price, "{0}任务支付".With(projectDemand.Title));
                    Haspaid(projectDemand);
                    payMoney = projectDemand.Price;
                }
                else
                {
                    if (projectBidding.Price < SiteConfig.PROJECTDMANDDEPOSIT)//开发者竞价小于雇主押金
                    {
                        _pointsLogService.ChangePoint(account, SiteConfig.PROJECTDMANDDEPOSIT - projectBidding.Price, "{0}任务多余押金返还".With(projectDemand.Title));
                        _developerPointsLogService.AddPoint(developer, projectBidding.Price, "{0}任务支付".With(projectDemand.Title));

                        Haspaid(projectDemand);
                    }
                    else//开发者竞价大于雇主押金，需判断用户剩余积分是否足够支付
                    {
                        if (projectBidding.Price - SiteConfig.PROJECTDMANDDEPOSIT < account.Points)
                        {
                            _pointsLogService.ChangePoint(account, SiteConfig.PROJECTDMANDDEPOSIT - projectBidding.Price, "{0}任务超出押金部分支付给开发者".With(projectDemand.Title));
                            _developerPointsLogService.AddPoint(developer, projectBidding.Price, "{0}任务支付".With(projectDemand.Title));

                            Haspaid(projectDemand);
                        }
                        else
                        {
                            throw new Exception("您的积分不足够支付开发者，请充值积分{0}".With(projectBidding.Price - SiteConfig.PROJECTDMANDDEPOSIT));
                        }
                    }
                    payMoney = projectBidding.Price;
                }
            }
            else
            {
                throw new Exception("您已经成功支付给开发者，请勿刷新页面避免重复支付！");
            }

            var projectPayLog = _projectPayLogService.GetObject(z => z.ProjectDemandId == projectDemand.Id);
            if (projectPayLog != null)
            {
                projectPayLog.Type = (int) ProjectPay_Type.已支付给开发者;
                projectPayLog.CompleteTime = DateTime.Now;
                _projectPayLogService.SaveObject(projectPayLog);
            }
            SuccessDemand(projectDemand, account);
            //发送邮件提醒开发者雇主已付款
            SendEmail sendEmail = new SendEmail(SendEmailType.ProjectDemandAgreePay);
            var sendEmailPara = new SendEmailParameter_ProjectDemandAgreePay(developer.Account.Email, developer.Account.UserName,
                payMoney,
                projectDemand.Title);
            sendEmail.Send(sendEmailPara, true);

            //发送站内信
            _messageBoxService.SendMessage(sendEmail.SentSubject, sendEmail.SentBody, developer.Id);
        }

        private void Haspaid(ProjectDemand projectDemand)//记录积分是否变更，防止重复变更积分
        {
            projectDemand.HasPaid = true;
            SaveObject(projectDemand);
        }

        public void SuccessDemand(ProjectDemand projectDemand, Account account)
        {
            AddSuccessDemandCount(projectDemand.AccountId);
            projectDemand.State = (int)ProjectDemand_State.已完成;
            projectDemand.FinishTime = DateTime.Now;
            SaveObject(projectDemand);
            CreditRating(account);//信誉升级

            var projectBidding = _projectBiddingService.GetObject(z => z.Id == projectDemand.WinBiddingId, new string[] { "Developer" });
            var developer = projectBidding.Developer;
            _projectBiddingService.AddSuccessBiddingCount(developer, developer.Account.UserName);
        }

        public void CommintTime(int projectDemandId)
        {
            var projectDemand = GetObject(projectDemandId);
            projectDemand.CommintTime = DateTime.Now;

            SaveObject(projectDemand);
        }

        /// <summary>
        /// 雇主信誉升级
        /// </summary>
        /// <param name="account">雇主</param>
        protected void CreditRating(Account account)
        {
            var count = account.SuccessProjectDemandCount;
            var creditRating = account.ProjectCreditRating;
            int sum = 0;
            for (int i = 0; i <= creditRating; i++)
            {
                sum += (i + 1) * i / 2;
            }

            if (count > sum)
            {
                account.ProjectCreditRating += 1;
                _accountService.SaveObject(account);
            }
        }

        public void DeductPoints(ProjectDemand projectDemand, Account account)
        {
            var price = projectDemand.Price == 0 ? SiteConfig.PROJECTDMANDDEPOSIT : projectDemand.Price;
            _pointsLogService.ChangePoint(account, -price, "发布【{0}】任务托管押金".With(projectDemand.Title));
            Step(projectDemand, (int)Project_Step.任务发布托管赏金);
            projectDemand.State = (int)ProjectDemand_State.竞标中;
            projectDemand.Mortgaged = true;
            SaveObject(projectDemand);
        }

      

        public override void SaveObject(ProjectDemand obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("ProjectDemand{2}：{0}（ID：{1}）", obj.AccountId, obj.Id, isInsert ? "新增" : "编辑");
        }

        public override void DeleteObject(ProjectDemand obj)
        {
            LogUtility.WebLogger.InfoFormat("ProjectDemand被删除：{0}（ID：{1}）", obj.AccountId, obj.Id);
            base.DeleteObject(obj);
        }

        public List<ProjectDemand> GetHotProjectDemandList()
        {
            return this.GetObjectList(1, 6, z => z.ProjectBidding == null, z => z.ProjectBiddings.Count, OrderingType.Descending, new string[] { "ProjectBiddings" });
        }
    }
}

