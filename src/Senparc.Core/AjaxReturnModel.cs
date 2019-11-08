using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core
{

    /// <summary>
    /// ajax返回模型
    /// </summary>
    public class AjaxReturnModel
    {
        public bool Success { get; set; }

        public string Msg { get; set; }
    }

    /// <summary>
    /// ajax返回模型
    /// </summary>
    /// <typeparam name="T">返回的对象</typeparam>
    public class AjaxReturnModel<T> : AjaxReturnModel
    {
        public T Data { get; set; }

        public AjaxReturnModel()
        {

        }

        public AjaxReturnModel(T data) : this()
        {
            this.Data = data;
        }
    }
}
