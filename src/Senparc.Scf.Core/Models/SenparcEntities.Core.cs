using System.Linq;
using System.Reflection;

namespace Senparc.Scf.Core.Models
{
    using Microsoft.EntityFrameworkCore;
    using Senparc.Scf.Core.Models;

    public partial class SenparcEntities : DbContext
    {
        public SenparcEntities(DbContextOptions<SenparcEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<AdminUserInfo> AdminUserInfos { get; set; }

        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public DbSet<FeedBack> FeedBacks { get; set; }

        public virtual DbSet<PointsLog> PointsLogs { get; set; }

        public virtual DbSet<AccountPayLog> AccountPayLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfigurationMapping());
            modelBuilder.ApplyConfiguration(new AdminUserInfoConfigurationMapping());
            modelBuilder.ApplyConfiguration(new FeedbackConfigurationMapping());
            modelBuilder.ApplyConfiguration(new AccountPayLogConfigurationMapping());
            modelBuilder.ApplyConfiguration(new PointsLogConfigurationMapping());
        }

        /// <summary>
        /// 
        /// </summary>
        private static readonly MethodInfo SetGlobalQueryMethodInfo = typeof(SenparcEntities)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            builder.Entity<T>().HasQueryFilter(z => z.Flag);
        }
    }
}
