using Senparc.ExtensionAreaTemplate.Models;
using Senparc.ExtensionAreaTemplate.Models.DatabaseModel;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Respository
{
    public class BaseRespository<T> : RepositoryBase<T>, IRepositoryBase<T> where T : EntityBase
    {
        public BaseRespository(SqlMyAppFinanceData db) : base(db)
        {
        }
    }
}
