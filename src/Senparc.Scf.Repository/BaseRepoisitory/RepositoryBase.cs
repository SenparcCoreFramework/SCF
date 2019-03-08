using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Senparc.Scf.Repository
{
    public class RepositoryBase<T> : DataBase, IRepositoryBase<T> where T : EntityBase //global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        protected string _entitySetName;

        public RepositoryBase() :
            this(null)
        {
        }

        public RepositoryBase(ISqlBaseFinanceData db) :
            base(db)
        {
            //System.Web.HttpContext.Current.Response.Write("-"+this.GetType().Name + "<br />");
            //DB = db ?? ObjectFactory.GetInstance<ISqlClientFinanceData>();//如果没有定义，取默认数据库
            _entitySetName = EntitySetKeys.Keys[typeof(T)];
        }

        //public BaseRepository() { }


        #region IBaseRepository<T> 成员


        public virtual bool IsInsert(T obj)
        {
            var entry = BaseDB.BaseDataContext.Entry(obj);
            return entry.State == EntityState.Added || entry.State == EntityState.Detached; //TODO:EF5、Core 验证正确性
            //return obj.EntityKey == null || obj.EntityKey.EntityKeyValues == null;

            //entry.IsKeySet
        }

        public virtual IQueryable<T> GeAll<TK>(Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            return BaseDB.BaseDataContext.Set<T>()
                        //.SqlQuery(sql)
                        //.CreateQuery<T>(sql)
                        .Includes(includes)
                        .OrderBy(orderBy, orderingType).AsQueryable();
        }

        public virtual PagedList<T> GetObjectList<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount, string[] includes = null)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            int skipCount = Senparc.Scf.Core.Utility.Extensions.GetSkipRecord(pageIndex, pageCount);
            int totalCount = -1;
            List<T> result = null;
            //var query = BaseDB.BaseDataContext.CreateQuery<T>(sql).Includes(includes).OrderBy(orderBy, orderingType);//.Includes(includes);
            IQueryable<T> resultList = BaseDB.BaseDataContext
                                        .Set<T>()
                                        //.CreateQuery<T>(sql)
                                        .Includes(includes)
                                        .Where(where)
                                        .OrderBy(orderBy, orderingType);//.Includes(includes);

            if (pageCount > 0 && pageIndex > 0)
            {
                resultList = resultList.Skip(skipCount).Take(pageCount);
                totalCount = this.ObjectCount(where, null); //whereList.Count();
            }
            //else
            //{
            //    resultList = query.;
            //}

            //try
            {
                result = resultList.ToList();
            }
            //catch (ArgumentException ex)//DbArithmeticExpression 参数必须具有数值通用类型。
            //{
            //    //通常是ordery by的问题 TODO:重新整理是否需要Skip等操作
            //    //result = query.Includes(includes)
            //    //            .OrderByIEnumerable(orderBy.Compile(), orderingType)//改用非延时地方法，效率最低
            //    //            .Skip(skipCount).Take(pageCount)
            //    //            .Where(where.Compile())//保险起见用where.Compile()，但是会影响效率
            //    //            .ToList();
            //    AdminLogUtility.WebLogger.Warn("EF ArgumentException", ex);
            //    throw;
            //}
            //catch (NotSupportedException ex)//System.Reflection.TargetException
            //{
            //    result = resultList.Where(where.Compile()).ToList();
            //    AdminLogUtility.WebLogger.Warn("EF NotSupportedException", ex);
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    result = resultList.Where(where.Compile()).ToList();
            //    AdminLogUtility.WebLogger.Warn("EF Exception", ex);
            //    throw;
            //}

            PagedList<T> list = new PagedList<T>(result, pageIndex, pageCount, totalCount, skipCount);
            return list;
        }



        public virtual async Task<PagedList<T>> GetObjectListAsync<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, int pageIndex, int pageCount, string[] includes = null)
        {
            int skipCount = Senparc.Scf.Core.Utility.Extensions.GetSkipRecord(pageIndex, pageCount);
            int totalCount = -1;
            List<T> result = null;
            IQueryable<T> resultList = BaseDB.BaseDataContext
                                               .Set<T>()
                                               .Includes(includes)
                                               .Where(where)
                                               .OrderBy(orderBy, orderingType);//.Includes(includes);
            if (pageCount > 0 && pageIndex > 0)
            {
                resultList = resultList.Skip(skipCount).Take(pageCount);
                totalCount = this.ObjectCount(where, null);
            }

            result = await resultList.ToListAsync();

            PagedList<T> list = new PagedList<T>(result, pageIndex, pageCount, totalCount, skipCount);
            return list;
        }





        public virtual T GetFirstOrDefaultObject(Expression<Func<T, bool>> where, string[] includes = null)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            return BaseDB.BaseDataContext
                 .Set<T>()
                 //.CreateQuery<T>(sql)
                 .Includes(includes).FirstOrDefault(where);
        }

        public virtual T GetFirstOrDefaultObject<TK>(Expression<Func<T, bool>> where, Expression<Func<T, TK>> orderBy, OrderingType orderingType, string[] includes = null)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            return BaseDB.BaseDataContext
                 .Set<T>()
                 //.CreateQuery<T>(sql)
                 .Includes(includes).Where(where).OrderBy(orderBy, orderingType).FirstOrDefault();
        }

        public virtual T GetObjectById(int id)
        {
            T obj = BaseDB.BaseDataContext
                .Set<T>()
                .Find(id);

            return obj;
        }

        public virtual T GetObjectById(long id)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            //T obj = BaseDB.BaseDataContext
            //     .Set<T>()
            //    //.CreateQuery<T>(sql)
            //     .Includes(includes).Where("it.Id = @id", new ObjectParameter("id", id)).FirstOrDefault();
            T obj = BaseDB.BaseDataContext.Set<T>().Find(id);
            return obj;
        }

        //public virtual T GetObjectByGuid(Guid guid, string[] includes = null)
        //{
        //    string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
        //    T obj = BaseDB.BaseDataContext
        //         .Set<T>()
        //        //.CreateQuery<T>(sql)
        //         .Includes(includes).Where("it.Guid = @guid", new ObjectParameter("guid", guid)).FirstOrDefault();
        //    return obj;
        //}

        public virtual int ObjectCount(Expression<Func<T, bool>> where, string[] includes = null)
        {
            string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            int count = 0;
            var query = BaseDB.BaseDataContext
                 .Set<T>()
                 //.CreateQuery<T>(sql)
                 .Includes(includes);
            //try
            {
                count = query.Count(where);
            }
            //catch (NotSupportedException ex)
            //{
            //    count = query.Count(where.Compile());
            //}
            //catch (Exception ex)
            //{
            //    count = query.Count(where.Compile());
            //    throw;
            //}
            return count;
        }

        public virtual decimal GetSum(Expression<Func<T, bool>> where, Func<T, decimal> sum, string[] includes = null)
        {
            //string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
            var result = BaseDB.BaseDataContext
                 .Set<T>()
                 //.CreateQuery<T>(sql)
                 .Includes(includes).Where(where).Sum(sum);
            return result;
        }

        //public virtual object ObjectSum(Expression<Func<T, bool>> where, Func<T,object> sumBy, string[] includes)
        //{
        //    string sql = string.Format("SELECT VALUE c FROM {0} AS c ", _entitySetName);
        //    object result= _db.DataContext.CreateQuery<T>(sql).Includes(includes).Where(where).Sum(sumBy);
        //    return result;
        //}


        public virtual void Add(T obj)
        {
            BaseDB.BaseDataContext.Set<T>().Add(obj);
            this.SaveChanges();
        }

        public virtual void Update(T obj)
        {
            //_db.DataContext.ApplyPropertyChanges(_entitySetName, obj);
            this.SaveChanges();
        }

        public virtual void Save(T obj)
        {
            if (this.IsInsert(obj))
            {
                obj.AddTime = obj.LastUpdateTime = SystemTime.Now.LocalDateTime;
                this.Add(obj);
            }
            else
            {
                obj.LastUpdateTime = SystemTime.Now.LocalDateTime;
                this.Update(obj);
            }
        }

        public virtual void SaveChanges()
        {
            BaseDB.BaseDataContext.SaveChanges();//TODO: SaveOptions.
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="softDelete">是否使用软删除</param>
        public virtual void Delete(T obj, bool softDelete = false)
        {
            if (softDelete)
            {
                obj.Flag = true;//软删除
            }
            else
            {
                BaseDB.BaseDataContext.Set<T>().Remove(obj);//硬删除
            }
            this.SaveChanges();
        }

        //public virtual void DeleteAll(IEnumerable<T> objs)
        //{
        //    //foreach (var obj in objs)
        //    //{
        //    //    _db.DataContext.DeleteObject(obj);
        //    //}
        //    //this.SaveChanges();
        //    var list = objs.ToList();
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        this.Delete(list[i]);
        //    }
        //}

        #endregion
    }
}
