using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Areas;

namespace MyArea
{
    public class Register : IAreaRegister
    {
        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
        {
            builder.AddRazorPagesOptions(options =>
            {
            });

            SenparcTrace.SendCustomLog("系统启动", "完成 Area:MyArea 注册");
           
            return builder;
        }
    }
}
