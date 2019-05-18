using Senparc.Core.Models;
using Senparc.Scf.Repository;


namespace Senparc.Repository
{
    public interface IAccountPayLogRepository : IClientRepositoryBase<AccountPayLog>
    {
    }

    public class AccountPayLogRepository : ClientRepositoryBase<AccountPayLog>, IAccountPayLogRepository
    {

    }
}

