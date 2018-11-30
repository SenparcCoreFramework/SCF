using System;

namespace Senparc.Core.Utility
{
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public class AutoSetCacheAttribute : Attribute
    {
        public AutoSetCacheAttribute()
        {
        }
    }
}