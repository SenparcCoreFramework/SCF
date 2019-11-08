using Senparc.Scf.Core.Models.VD;

namespace Senparc.Areas.User.Models.VD
{
    public interface IBaseUserVD : IBaseVD
    {

    }

    public class BaseUserVD : BaseVD, IBaseUserVD
    {
    
    }
}