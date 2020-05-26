using Microsoft.EntityFrameworkCore;
using Senparc.Core.Models;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Service.SystemEntities
{
    public class SystemServiceEntities : XscfDatabaseDbContext
    {
        public override IXscfDatabase XscfDatabaseRegister => new Register();
        public SystemServiceEntities(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        #region 系统表

        public virtual DbSet<AdminUserInfo> AdminUserInfos { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        #endregion

    }
}
