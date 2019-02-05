using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Scf.Service
{
    public interface IBaseService<T> : IBaseServiceData where T : class, new()// global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        IBaseRepository<T> BaseRepository { get; set; }

        bool IsInsert(T obj);

        T GetObject(Expression<Func<T, bool>> where, string[] includes = null);

        T GetObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);

        Task<PagedList<T>> GetObjectListAsync<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where,
            Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);


        PagedList<T> GetFullList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);

        PagedList<T> GetObjectList<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);

        int GetCount(Expression<Func<T, bool>> where, string[] includes = null);

        decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum, string[] includes = null);

        /// <summary>
        /// 强制将实体设置为Modified状态
        /// </summary>
        /// <param name="obj"></param>
        void TryDetectChange(T obj);

        void DeleteObject(Expression<Func<T, bool>> predicate);

        void SaveObject(T obj);

        void DeleteObject(T obj);

        void DeleteAll(IEnumerable<T> objects);
    }
}