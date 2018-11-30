using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface IAccountRepository : IBaseClientRepository<Account>
    {
    }

    public class AccountRepository : BaseClientRepository<Account>, IAccountRepository
    {

    }
}

