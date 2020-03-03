using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using Senparc.Scf.Service;
using System;

namespace Senparc.Service
{
    public interface IBaseClientService<T> : IClientServiceBase<T> where T : EntityBase//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        IClientRepositoryBase<T> BaseClientRepository { get; }

        ///// <summary>
        ///// 开启事务
        ///// </summary>
        ///// <returns></returns>
        //IDbContextTransaction BeginTransaction();
    }


    public class BaseClientService<T> : ServiceBase<T>, IBaseClientService<T> where T : EntityBase//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
      

        public BaseClientService(IClientRepositoryBase<T> repo, IServiceProvider serviceProvider)
            : base(repo, serviceProvider)
        {

        }
    }
}