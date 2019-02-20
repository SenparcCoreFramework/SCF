using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Scf.Repository
{
    public interface IRepositoryBase<T> : IDataBase, IAutoDI where T : class, IEntityBase// global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        bool IsInsert(T obj);

        IQueryable<T> GeAll<TK>(Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="where">搜索条件</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount">当pageCount小于等于0时不分页</param>
        /// <returns></returns>
        PagedList<T> GetObjectList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount, string[] includes = null);

        Task<PagedList<T>> GetObjectListAsync<TK>(Expression<Func<T, bool>> where,
          Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount,
          string[] includes = null);


        T GetFirstOrDefaultObject(Expression<Func<T, bool>> where, string[] includes = null);

        T GetFirstOrDefaultObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null);

        int ObjectCount(Expression<Func<T, bool>> where, string[] includes = null);

        decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum, string[] includes = null);

        void Add(T obj);

        void Update(T obj);

        /// <summary>
        /// 此方法会自动判断应当执行更新(Update)还是添加(Add)
        /// </summary>
        /// <param name="obj"></param>
        void Save(T obj);

        void Delete(T obj, bool softDelete = false);

        void SaveChanges();
    }
}