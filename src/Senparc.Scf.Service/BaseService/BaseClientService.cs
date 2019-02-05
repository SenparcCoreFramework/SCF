using Microsoft.EntityFrameworkCore.Storage;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Scf.Service
{
    public interface IBaseClientService<T> : IBaseService<T> where T : EntityBase, new()//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        IBaseClientRepository<T> BaseClientRepository { get; }

        /// <summary>
        /// 开启事物
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();
    }


    public class BaseClientService<T> : BaseService<T>, IBaseClientService<T> where T : EntityBase, new()//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        public IBaseClientRepository<T> BaseClientRepository
        {
            get
            {
                return BaseRepository as IBaseClientRepository<T>;
            }
        }

        public BaseClientService(IBaseClientRepository<T> repo)
            : base(repo)
        {
        }

        /// <summary>
        /// 开启事物
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            return BaseData.BaseDB.BaseDataContext.Database.BeginTransaction();
        }
    }
}