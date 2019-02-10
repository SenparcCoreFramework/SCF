using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.DI
{
    /// <summary>
    /// 所有需要自动扫描进行依赖注入的基础接口
    /// <para>默认 DI 使用 AddScoped 方法，如果需要强制使用其他方法，请在子类上使用 [AutoDIType(typeName)] 特性标签</para>
    /// </summary>
    public interface IAutoDI
    {
    }
}
