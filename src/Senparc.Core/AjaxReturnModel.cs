using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core
{
    public class AjaxReturnModel
    {
        public bool Success { get; set; }

        public string Msg { get; set; }
    }

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
