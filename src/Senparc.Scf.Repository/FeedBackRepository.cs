using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IFeedBackRepository : IBaseClientRepository<FeedBack>
    {
    }

    public class FeedBackRepository : BaseClientRepository<FeedBack>, IFeedBackRepository
    {
    }
}