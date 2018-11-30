using Senparc.Core.Models;

namespace Senparc.Repository
{
    public interface IAdminUserInfoRepository : IBaseClientRepository<AdminUserInfo>
    {
    }

    public class AdminUserInfoRepository : BaseClientRepository<AdminUserInfo>, IAdminUserInfoRepository
    {

    }
}

