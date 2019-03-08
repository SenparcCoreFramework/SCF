using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Areas
{
    /// <summary>
    /// Area 的 Register 接口
    /// </summary>
   public interface IAreaRegister
    {
        IMvcBuilder AuthorizeConfig(IMvcBuilder builder);
    }
}
