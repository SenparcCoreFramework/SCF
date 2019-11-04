using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin.Filters
{
    public class CustomerResourceFilterAttribute : Attribute, Microsoft.AspNetCore.Mvc.Filters.IFilterMetadata
    {
        public string ResourceCode { get; set; }

        public CustomerResourceFilterAttribute()
        {

        }

        public CustomerResourceFilterAttribute(string resuouceCode)
        {
            ResourceCode = resuouceCode;
        }
    }
}
