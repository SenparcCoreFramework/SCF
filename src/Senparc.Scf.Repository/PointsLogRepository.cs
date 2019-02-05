using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IPointsLogRepository : IBaseClientRepository<PointsLog>
    {
    }

    public class PointsLogRepository : BaseClientRepository<PointsLog>, IPointsLogRepository
    {

    }
}

