using System.Linq;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Senparc.Scf.Utility
{
    public static class ModelStateDictionaryExtension
    {
        /// <summary>
        /// 获取第一个错误提示
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string FirstErrorMessage (this ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary.IsValid)
            {
                return "";
            }
            return modelStateDictionary.Values.FirstOrDefault (z => z.Errors.Count > 0).Errors [0].ErrorMessage;
        }
    }

}
