using System;

namespace Senparc.Scf.Core.Utility
{
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public class AutoSetCacheAttribute : Attribute
    {
        public AutoSetCacheAttribute()
        {
        }
    }
}