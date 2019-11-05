using AutoMapper;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Scf.Service
{
    public interface IServiceBase<T> : IServiceDataBase, IAutoDI where T : class, IEntityBase// global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        IMapper Mapper { get; set; } //TODO: add in to Wapper

        IRepositoryBase<T> RepositoryBase { get; set; }

        bool IsInsert(T obj);

        T GetObject(Expression<Func<T, bool>> where,params string[] includes);

        T GetObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);

        Task<PagedList<T>> GetObjectListAsync<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where,
            Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);


        PagedList<T> GetFullList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);

        PagedList<T> GetObjectList<TK>(int pageIndex, int pageCount, Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);

        int GetCount(Expression<Func<T, bool>> where,params string[] includes);

        decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum,params string[] includes);

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