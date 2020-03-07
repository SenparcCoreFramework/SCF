using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Senparc.Core.Models
{

    public partial class SenparcEntities : SenparcEntitiesBase, ISenparcEntities
    {
        public SenparcEntities(DbContextOptions<SenparcEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        #region 系统表

        public virtual DbSet<AdminUserInfo> AdminUserInfos { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 系统表

            modelBuilder.ApplyConfiguration(new AdminUserInfoConfigurationMapping());
            modelBuilder.ApplyConfiguration(new FeedbackConfigurationMapping());

            #endregion

            #region 其他动态模块

            foreach (var databaseRegister in Senparc.Scf.XscfBase.Register.XscfDatabaseList)
            {
                databaseRegister.OnModelCreating(modelBuilder);
            }
            
            #endregion

            base.OnModelCreating(modelBuilder);
        }


    }
}
