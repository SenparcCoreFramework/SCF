using Microsoft.EntityFrameworkCore;
using Senparc.CO2NET.Trace;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.XscfBase.Attributes;
using System;
using System.Collections.Concurrent;
using System.Linq;

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

            //注册所有 XscfAutoConfigurationMapping 动态模块
            foreach (var autoConfigurationMapping in Senparc.Scf.XscfBase.Register.XscfAutoConfigurationMappingList)
            {
                if (autoConfigurationMapping == null)
                {
                    continue;
                }
                modelBuilder.ApplyConfiguration(autoConfigurationMapping);
            }

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
