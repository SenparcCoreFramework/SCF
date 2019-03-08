using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IAccountPayLogRepository : IClientRepositoryBase<AccountPayLog>
    {
    }

    public class AccountPayLogRepository : ClientRepositoryBase<AccountPayLog>, IAccountPayLogRepository
    {

    }
}

