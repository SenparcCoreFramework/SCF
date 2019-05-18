using Senparc.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Repository
{
    public interface IAccountRepository : IClientRepositoryBase<Account>
    {
    }

    public class AccountRepository : ClientRepositoryBase<Account>, IAccountRepository
    {

    }
}

