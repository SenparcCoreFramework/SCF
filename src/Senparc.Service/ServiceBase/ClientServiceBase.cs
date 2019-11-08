using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using Senparc.Scf.Service;
using System;

namespace Senparc.Service
{
    public interface IClientServiceBase<T> : IServiceBase<T> where T : EntityBase//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        IClientRepositoryBase<T> BaseClientRepository { get; }

        ///// <summary>
        ///// 开启事物
        ///// </summary>
        ///// <returns></returns>
        //IDbContextTransaction BeginTransaction();
    }


    public class ClientServiceBase<T> : ServiceBase<T>, IClientServiceBase<T> where T : EntityBase//global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        public IClientRepositoryBase<T> BaseClientRepository
        {
            get
            {
                return RepositoryBase as IClientRepositoryBase<T>;
            }
        }

        public ClientServiceBase(IClientRepositoryBase<T> repo, IServiceProvider serviceProvider)
            : base(repo, serviceProvider)
        {

        }
    }
}