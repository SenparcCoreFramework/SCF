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

            ConcurrentDictionary<Type, int> types = new ConcurrentDictionary<Type, int>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                var aTypes = a.GetTypes();
                foreach (var t in aTypes)
                {
                    if (t.IsAbstract)
                    {
                        continue;
                    }

                    if (t.GetCustomAttributes(true).FirstOrDefault(z => z is XscfAutoConfigurationMappingAttribute) != null)
                    {
                        types[t] = 1;
                    }
                }
            }
            SenparcTrace.SendCustomLog("扫描 XscfAutoConfigurationMapping", "总数：" +
    string.Join(",", types.Select(z => z.Key.GetType().FullName)));


            //注册所有 XscfAutoConfigurationMapping 动态模块
            SenparcTrace.SendCustomLog("注册 XscfAutoConfigurationMapping", "总数：" +
                string.Join(",", Senparc.Scf.XscfBase.Register.XscfAutoConfigurationMappingList.Select(z => z.GetType().FullName)));
            foreach (var autoConfigurationMapping in Senparc.Scf.XscfBase.Register.XscfAutoConfigurationMappingList)
            {
                SenparcTrace.SendCustomLog("注册 XscfAutoConfigurationMapping", autoConfigurationMapping.GetType().FullName);
                modelBuilder.ApplyConfiguration(autoConfigurationMapping);
            }

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
