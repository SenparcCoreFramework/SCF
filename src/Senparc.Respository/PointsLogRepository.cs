using Senparc.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Repository
{
    public interface IPointsLogRepository : IClientRepositoryBase<PointsLog>
    {
    }

    public class PointsLogRepository : ClientRepositoryBase<PointsLog>, IPointsLogRepository
    {

    }
}

