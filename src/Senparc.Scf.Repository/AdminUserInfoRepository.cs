using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Repository
{
    public interface IAdminUserInfoRepository : IBaseClientRepository<AdminUserInfo>
    {
    }

    public class AdminUserInfoRepository : BaseClientRepository<AdminUserInfo>, IAdminUserInfoRepository
    {

    }
}

