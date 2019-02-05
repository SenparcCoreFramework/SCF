using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IAccountRepository : IBaseClientRepository<Account>
    {
    }

    public class AccountRepository : BaseClientRepository<Account>, IAccountRepository
    {

    }
}

