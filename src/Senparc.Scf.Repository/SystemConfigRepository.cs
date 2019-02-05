using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface ISystemConfigRepository : IBaseClientRepository<SystemConfig>
    {
    }

    public class SystemConfigRepository : BaseClientRepository<SystemConfig>, ISystemConfigRepository
    {

    }
}

