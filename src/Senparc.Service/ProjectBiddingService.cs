using Senparc.Core.Models;
using Senparc.Log;

namespace Senparc.Service
{
    public interface IProjectBiddingService : IBaseClientService<ProjectBidding>
    {
        /// <summary>
        /// 统计竞标数量
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="userName"></param>
        void AddBiddingCount(int developerId, string userName);
        /// <summary>
        /// 统计完成竞标数量
        /// </summary>
        /// <param name="developer"></param>
        /// <param name="userName"></param>
        void AddSuccessBiddingCount(Developer developer, string userName);
        /// <summary>
        /// 开发结束
        /// </summary>
        /// <param name="id">ProjectBiddingId</param>
        void Finish(int id);
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="projectBidding"></param>
        /// <param name="grade">评分</param>
        /// <param name="comment">评论内容</param>
        void AddComment(ProjectBidding projectBidding, int grade, string comment);
        /// <summary>
        /// 判断开发者是否竞标
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="projectDemandId"></param>
        /// <returns></returns>
        bool ExistBidding(int developerId, int projectDemandId);
    }

    public class ProjectBiddingService : BaseClientService<ProjectBidding>, IProjectBiddingService
    {
        private IDeveloperService _developerService;
        public ProjectBiddingService(IProjectBiddingRepository ProjectBiddingRepo, IDeveloperService developerService)
            : base(ProjectBiddingRepo)
        {
            _developerService = developerService;
        }

        public void AddBiddingCount(int developerId, string userName)
        {
            var developer = _developerService.GetObject(developerId);
            developer.ProjectBiddingCount++;
            _developerService.SaveObject(developer, userName);
        }

        public void AddSuccessBiddingCount(Developer developer, string userName)
        {
            developer.SuccessProjectBiddingCount++;
            UpLever(developer);//开发者升级
            _developerService.SaveObject(developer, userName);
        }

        public void Finish(int id)
        {
            var projectBidding = GetObject(id);

            projectBidding.IsFinished = true;
            SaveObject(projectBidding);
        }

        public void AddComment(ProjectBidding projectBidding, int grade, string comment)
        {
            projectBidding.CommentGrade = grade;
            projectBidding.CommentText = comment;
            SaveObject(projectBidding);
        }

        /// <summary>
        /// 开发者升级
        /// </summary>
        /// <param name="developer">开发者</param>
        protected void UpLever(Developer developer)
        {
            var count = developer.SuccessProjectBiddingCount;
            var level = developer.Level;
            int sum = 0;
            for (int i = 0; i <= level; i++)
            {
                sum += (i + 1) * i / 2;
            }

            if (count > sum)
            {
                developer.Level += 1;
            }
        }

        public override void SaveObject(ProjectBidding obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("ProjectBidding{2}：{0}（ID：{1}）", obj.Developer.Name, obj.Id, isInsert ? "新增" : "编辑");
        }

        public override void DeleteObject(ProjectBidding obj)
        {
            LogUtility.WebLogger.InfoFormat("ProjectBidding被删除：{0}（ID：{1}）", obj.Developer.Name, obj.Id);
            base.DeleteObject(obj);
        }

        public bool ExistBidding(int developerId, int projectDemandId)
        {
            return this.GetObject(z => z.DeveloperId == developerId && z.ProjectDemandId == projectDemandId) != null;
        }
    }
}

