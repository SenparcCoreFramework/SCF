using System.Linq;
using System.Reflection;

namespace Senparc.Core.Models
{
    using Microsoft.EntityFrameworkCore;
    using Senparc.Core.Models.DataBaseModel;
    using Senparc.Scf.Core.Models;
    using Senparc.Scf.Core.Models.DataBaseModel;
    using System;
    using System.Linq.Expressions;

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

            base.OnModelCreating(modelBuilder);
        }


    }
}
