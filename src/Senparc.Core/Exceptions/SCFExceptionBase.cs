using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Exceptions
{
    public class SCFExceptionBase : CO2NET.Exceptions.BaseException
    {
        public SCFExceptionBase(string message, bool logged = false) : base(message, logged)
        {
        }

        public SCFExceptionBase(string message, Exception inner, bool logged = false) : base(message, inner, logged)
        {
        }
    }
}
