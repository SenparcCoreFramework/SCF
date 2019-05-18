using Senparc.Core.Models;
using Senparc.Scf.Repository;

namespace Senparc.Repository
{
    public interface IAdminUserInfoRepository : IClientRepositoryBase<AdminUserInfo>
    {
    }

    public class AdminUserInfoRepository : ClientRepositoryBase<AdminUserInfo>, IAdminUserInfoRepository
    {

    }
}

