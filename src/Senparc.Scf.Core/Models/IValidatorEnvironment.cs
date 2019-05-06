using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// 作为需要进行视图验证的基础接口（如Controller、PageModel）
    /// </summary>
    public interface IValidatorEnvironment
    {
        /// <summary>
        /// Controller 及 PageModel 中的 ModelState 对象
        /// </summary>
        ModelStateDictionary ModelState { get; }

        /// <summary>
        /// HttpContext
        /// </summary>
        HttpContext HttpContext { get; }
    }
}
