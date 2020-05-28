using Microsoft.EntityFrameworkCore;
using Senparc.Core.Models;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Service
{
    /// <summary>
    /// 当前 Entities 只为对接 Service 的 Register 而存在（必须继承自 XscfDatabaseDbContext），没有特别的意义。
    /// TODO：让 SenparcEntities 继承 XscfDatabaseDbContext，并提供 Senparc.Core 的 Register。
    /// </summary>
    public class SystemServiceEntities : XscfDatabaseDbContext
    {
        public override IXscfDatabase XscfDatabaseRegister => new Register();
        public SystemServiceEntities(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
