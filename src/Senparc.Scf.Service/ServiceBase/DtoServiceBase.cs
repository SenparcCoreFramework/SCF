using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Dto;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.Service.ServiceBase
{
    public class DtoServiceBase<TEntity, TEntityDto,TEntityInsertObj,TEntityEditObj> : ServiceBase<TEntity>
        where TEntity : class, IEntityBase, new()
        where TEntityDto : IDtoBase
    {
        public DtoServiceBase(IRepositoryBase<TEntity> repo)
            : base(repo)
        {

        }


        public virtual bool IsInsert(TEntity obj)
        {
            return RepositoryBase.IsInsert(obj);
        }

        public TEntity GetObject(Expression<Func<TEntity, bool>> where, string[] includes = null)
        {
            return RepositoryBase.GetFirstOrDefaultObject(where, includes);
        }

        public TEntity GetObject<TK>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return RepositoryBase.GetFirstOrDefaultObject(where, orderBy, orderingType, includes);
        }

        public PagedList<TEntity> GetFullList<TK>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return this.GetObjectList(0, 0, where, orderBy, orderingType, includes);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">每页数量</param>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="orderingType">正序|倒叙</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual PagedList<TEntity> GetObjectList<TK>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return RepositoryBase.GetObjectList(where, orderBy, orderingType, pageIndex, pageCount, includes);
        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageCount">每页数量</param>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="orderingType">正序|倒叙</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<TEntity>> GetObjectListAsync<TK>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            return await RepositoryBase.GetObjectListAsync(where, orderBy, orderingType, pageIndex, pageCount, includes);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> where, string[] includes = null)
        {
            return RepositoryBase.ObjectCount(where, includes);
        }

        public virtual decimal GetSum(Expression<Func<TEntity, bool>> where, Func<TEntity, decimal> sum, string[] includes = null)
        {
            return RepositoryBase.GetSum(where, sum, includes);
        }

        /// <summary>
        /// 强制将实体设置为Modified状态
        /// </summary>
        /// <param name="obj"></param>
        public virtual void TryDetectChange(TEntity obj)
        {
            if (!IsInsert(obj))
            {
                RepositoryBase.BaseDB.BaseDataContext.Entry(obj).State = EntityState.Modified;
            }
        }

        public virtual void SaveObject(TEntity obj)
        {
            if (RepositoryBase.BaseDB.ManualDetectChangeObject)
            {
                TryDetectChange(obj);
            }
            RepositoryBase.Save(obj);
        }

        public virtual void DeleteObject(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity obj = GetObject(predicate);
            DeleteObject(obj);
        }

        public virtual void DeleteObject(TEntity obj)
        {
            RepositoryBase.Delete(obj);
        }

        public virtual void DeleteAll(IEnumerable<TEntity> objects)
        {
            var list = objects.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DeleteObject(list[i]);
            }
        }

        //TODO: 提供异步版本
    }
}
