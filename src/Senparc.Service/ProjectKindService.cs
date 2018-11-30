using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Repository;
using Senparc.Log;

namespace Senparc.Service
{
    public interface IProjectKindService : IBaseClientService<ProjectKind>
    {
        Dictionary<ProjectKind, List<ProjectKind>> GetKindTree();
        /// <summary>
        /// 统计该分类的任务数
        /// </summary>
        /// <param name="kindId">任务分类Id</param>
        void ProjectDemandCount(int kindId);
        /// <summary>
        /// 撤销任务时该分类任务数减1
        /// </summary>
        /// <param name="kindId">任务分类Id</param>
        void ReduceProjectDemandCount(int kindId);
        /// <summary>
        /// Init方法添加一级目录
        /// </summary>
        void CreateOneLevelKind();
        /// <summary>
        /// Init方法添加二级目录
        /// </summary>
        /// <param name="names">二级目录名称数组</param>
        /// <param name="parentName">一级目录名称</param>
        void CreateTwoLevelKind(string[] names, ProjectKind_List parentName);
        /// <summary>
        /// 获取所有父级任务类型
        /// </summary>
        /// <returns></returns>
        List<ProjectKind> GetParentProjectKindList();
    }

    public class ProjectKindService : BaseClientService<ProjectKind>, IProjectKindService
    {
        public ProjectKindService(IProjectKindRepository projectKindRepo)
            : base(projectKindRepo)
        {

        }

        public Dictionary<ProjectKind, List<ProjectKind>> GetKindTree()
        {
            Dictionary<ProjectKind, List<ProjectKind>> result =
                new Dictionary<ProjectKind, List<ProjectKind>>();
            var fullList = base.GetFullList(z => true, z => z.Id, OrderingType.Ascending);

            //获取顶部菜单
            foreach (var topKind in fullList.Where(z => z.Level == 0).OrderBy(z => z.DisplayOrder))
            {
                var subKinds = fullList.Where(z => z.ParentId == topKind.Id).OrderBy(z => z.DisplayOrder).ToList();
                result[topKind] = subKinds;
            }

            return result;
        }

        public List<ProjectKind> GetKindList(int? id)
        {
            var KindList = new List<ProjectKind>();

            if (id.HasValue)
            {
                KindList = GetFullList(z => z.ParentId == id, z => z.Id, OrderingType.Descending);
            }
            else
            {
                KindList = GetFullList(z => z.Level == 0, z => z.Id, OrderingType.Descending);
            }

            return KindList;
        }

        public void ProjectDemandCount(int kindId)
        {
            var projectKind = GetObject(kindId);
            projectKind.ProjectDemandCount++;
            SaveObject(projectKind);
        }

        public void ReduceProjectDemandCount(int kindId)
        {
            var projectKind = GetObject(kindId);
            projectKind.ProjectDemandCount--;
            SaveObject(projectKind);
        }

        public void CreateOneLevelKind()
        {
            string[] names = Enum.GetNames(typeof(ProjectKind_List));
            for (int i = 0; i < names.Count(); i++)
            {
                var projectKind = new ProjectKind()
                {
                    Name = names[i],
                    Description = names[i],
                    ProjectDemandCount = 0,
                    Level = 0,
                    DisplayOrder = 0,
                    Show = true,
                    AddTime = DateTime.Now
                };
                var name = names[i];
                var notExist = GetObject(z => z.Name == name) == null;
                if (notExist)
                {
                    SaveObject(projectKind);
                }
            }
        }

        public void CreateTwoLevelKind(string[] names, ProjectKind_List parentName)
        {
            var parent = parentName.ToString();
            var parentId = GetObject(z => z.Name == parent).Id;

            for (int i = 0; i < names.Count(); i++)
            {
                var projectKind = new ProjectKind()
                {
                    Name = names[i],
                    Description = names[i],
                    ParentId = parentId,
                    ProjectDemandCount = 0,
                    Level = 1,
                    DisplayOrder = 0,
                    Show = true,
                    AddTime = DateTime.Now
                };

                var name = names[i];
                var notExist = GetObject(z => z.Name == name) == null;
                if (notExist)
                {
                    SaveObject(projectKind);
                }
            }
        }

        public override void SaveObject(ProjectKind obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("ProjectKind{2}：{0}（ID：{1}）", obj.Name, obj.Id, isInsert ? "新增" : "编辑");
        }
        public override void DeleteObject(ProjectKind obj)
        {
            LogUtility.WebLogger.InfoFormat("ProjectKind被删除：{0}（ID：{1}）", obj.Name, obj.Id);
            base.DeleteObject(obj);
        }

        public List<ProjectKind> GetParentProjectKindList()
        {
            return GetFullList(z => z.Level == 0 && z.Show, z => z.Id, OrderingType.Descending);
        }
    }
}

