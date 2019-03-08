using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IAccountRepository : IClientRepositoryBase<Account>
    {
    }

    public class AccountRepository : ClientRepositoryBase<Account>, IAccountRepository
    {

    }
}

