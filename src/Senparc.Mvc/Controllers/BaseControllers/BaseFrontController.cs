using Microsoft.AspNetCore.Authorization;
using Senparc.Core.Extensions;

namespace Senparc.Mvc.Controllers
{
    [UserAuthorize("UserAnonymous")]
    [AllowAnonymous]
    public class BaseFrontController : BaseController
    {

    }
}
