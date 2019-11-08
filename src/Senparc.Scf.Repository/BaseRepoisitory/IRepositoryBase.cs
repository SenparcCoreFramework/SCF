using Microsoft.EntityFrameworkCore.Storage;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Scf.Repository
{
    public interface IRepositoryBase<T> : IDataBase, IAutoDI where T : class, IEntityBase// global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        bool IsInsert(T obj);

        IQueryable<T> GeAll<TK>(Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="where">搜索条件</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount">当pageCount小于等于0时不分页</param>
        /// <returns></returns>
        PagedList<T> GetObjectList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount,params string[] includes);

        Task<PagedList<T>> GetObjectListAsync<TK>(Expression<Func<T, bool>> where,
          Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount,
         params string[] includes);


        T GetFirstOrDefaultObject(Expression<Func<T, bool>> where,params string[] includes);

        T GetFirstOrDefaultObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType,params string[] includes);

        int ObjectCount(Expression<Func<T, bool>> where,params string[] includes);

        decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum,params string[] includes);

        void Add(T obj);

        void Update(T obj);

        /// <summary>
        /// 此方法会自动判断应当执行更新(Update)还是添加(Add)
        /// </summary>
        /// <param name="obj"></param>
        void Save(T obj);

        void Delete(T obj, bool softDelete = false);

        void SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task SaveAsync(T obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="softDelete"></param>
        /// <returns></returns>
        Task DeleteAsync(T obj, bool softDelete = false);

        Task SaveChangesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<T> GetFirstOrDefaultObjectAsync(Expression<Func<T, bool>> where,params string[] includes);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="softDelete"></param>
        /// <returns></returns>
        Task DeleteAllAsync(IEnumerable<T> objs, bool softDelete = false);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="softDelete"></param>
        /// <returns></returns>
        Task AddAllAsync(IEnumerable<T> objs);

        /// <summary>
        /// 动态字段排序
        /// </summary>
        /// <param name="where"></param>
        /// <param name="OrderbyField">xxx desc, bbb asc</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<PagedList<T>> GetObjectListAsync(Expression<Func<T, bool>> where, string OrderbyField, int pageIndex, int pageCount,params string[] includes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        Task SaveObjectListAsync(IEnumerable<T> objs);

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// 开启事物
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        void BeginTransaction(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        void RollbackTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();
    }
}