using Senparc.Scf.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core
{
    /// <summary>
    /// SCF 未安装
    /// </summary>
    public class ScfUninstallException : SCFExceptionBase
    {
        public ScfUninstallException(string message, bool logged = false) : base(message, logged)
        {
        }
    }
}
