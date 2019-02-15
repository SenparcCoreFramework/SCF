using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IPointsLogRepository : IClientRepositoryBase<PointsLog>
    {
    }

    public class PointsLogRepository : ClientRepositoryBase<PointsLog>, IPointsLogRepository
    {

    }
}

