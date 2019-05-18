using Senparc.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Repository
{
    public interface ISystemConfigRepository : IClientRepositoryBase<SystemConfig>
    {
    }

    public class SystemConfigRepository : ClientRepositoryBase<SystemConfig>, ISystemConfigRepository
    {
        
    }
}

