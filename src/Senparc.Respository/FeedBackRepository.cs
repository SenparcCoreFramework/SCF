using Senparc.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Repository
{
    public interface IFeedBackRepository : IClientRepositoryBase<FeedBack>
    {
    }

    public class FeedBackRepository : ClientRepositoryBase<FeedBack>, IFeedBackRepository
    {
    }
}