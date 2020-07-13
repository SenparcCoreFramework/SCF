﻿/* 
 * 特别注意：
 * 当前注册类是比较特殊的底层系统支持模块，
 * 其中加入了一系列特殊处理的代码，并不适合所有模块使用，
 * 如果需要学习扩展模块，请参考 【Senparc.ExtensionAreaTemplate】 项目的 Register.cs 文件！
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Core.Models;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Service
{
   public class Register : XscfRegisterBase,
        IXscfRegister, //注册 XSCF 基础模块接口（必须）
        IXscfDatabase  //注册 XSCF 模块数据库（按需选用）
    {
        #region IXscfRegister 接口

        public override string Name => "SenparcCoreFramework.Services";

        public override string Uid => SiteConfig.SYSTEM_XSCF_MODULE_SERVICE_UID;// "00000000-0000-0000-0000-000000000001";

        public override string Version => "0.1.0-beta4";

        public override string MenuName => "SCF 系统服务运行核心";

        public override string Icon => "fa fa-university";

        public override string Description => "这是系统服务核心模块，主管基础数据结构和网站核心运行数据，请勿删除此模块。如果你实在忍不住，请务必做好数据备份。";

        public override IList<Type> Functions => new Type[] { };


        public override IServiceCollection AddXscfModule(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<XscfModuleServiceExtension>();
            services.AddScoped<SystemServiceEntities>();

            return base.AddXscfModule(services, configuration);
        }

        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            XscfModuleServiceExtension xscfModuleServiceExtension = serviceProvider.GetService<XscfModuleServiceExtension>();
            //SenparcEntities senparcEntities = (SenparcEntities)xscfModuleServiceExtension.BaseData.BaseDB.BaseDataContext;
            SenparcEntities senparcEntities = (SenparcEntities)xscfModuleServiceExtension.BaseData.BaseDB.BaseDataContext;

            //更新数据库
            var pendingMigs = await senparcEntities.Database.GetPendingMigrationsAsync();
            if (pendingMigs.Count() > 0)
            {
                senparcEntities.ResetMigrate();//重置合并状态
                senparcEntities.Migrate();//进行合并
            }

            //更新数据库（目前不使用 SystemServiceEntities 存放数据库模型）
            //await base.MigrateDatabaseAsync<SystemServiceEntities>(serviceProvider);

            var systemModule = xscfModuleServiceExtension.GetObject(z => z.Uid == this.Uid);
            if (systemModule == null)
            {
                //只在未安装的情况下进行安装，InstallModuleAsync会访问到此方法，不做判断可能会引发死循环。
                //常规模块中请勿在此方法中自动安装模块！
                await xscfModuleServiceExtension.InstallModuleAsync(this.Uid).ConfigureAwait(false);
            }

            await base.InstallOrUpdateAsync(serviceProvider, installOrUpdate);
        }

        public override Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            //TODO：应该提供一个 BeforeUninstall 方法，阻止卸载。

            return base.UninstallAsync(serviceProvider, unsinstallFunc);
        }

        #endregion

        #region IXscfDatabase 接口

        public string DatabaseUniquePrefix => "";//特殊情况：没有前缀
        public Type XscfDatabaseDbContextType => typeof(SystemServiceEntities);


        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            //已在 SenparcEntities 中完成

        }

        public void AddXscfDatabaseModule(IServiceCollection services)
        {
            #region 历史解决方案参考信息
            /* 参考信息
             *      错误信息：
             *          中文：EnableRetryOnFailure 解决短暂的数据库连接失败
             *          英文：Win32Exception: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond
             *                InvalidOperationException: An exception has been raised that is likely due to a transient failure. Consider enabling transient error resiliency by adding 'EnableRetryOnFailure()' to the 'UseSqlServer' call.
             *      问题解决方案说明：https://www.colabug.com/2329124.html
             */

            /* 参考信息
             *      错误信息：
             *          中文：EnableRetryOnFailure 解决短暂的数据库连接失败
             *          英文：Win32Exception: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond
             *                InvalidOperationException: An exception has been raised that is likely due to a transient failure. Consider enabling transient error resiliency by adding 'EnableRetryOnFailure()' to the 'UseSqlServer' call.
             *      问题解决方案说明：https://www.colabug.com/2329124.html
             */
            #endregion

            //SenparcEntities 工厂配置
            Func<IServiceProvider, SenparcEntities> senparcEntitiesImplementationFactory = s =>
                new SenparcEntities(new DbContextOptionsBuilder<SenparcEntities>()
                    .UseSqlServer(Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString,
                                    b => base.DbContextOptionsAction(b, "Senparc.Service")/*从当前程序集读取*/)
                    .Options);
            services.AddScoped(senparcEntitiesImplementationFactory);
            services.AddScoped<ISenparcEntities>(senparcEntitiesImplementationFactory);
            services.AddScoped<SenparcEntitiesBase>(senparcEntitiesImplementationFactory);

            //SystemServiceEntities 工厂配置（实际不会用到）
            Func<IServiceProvider, SystemServiceEntities> systemServiceEntitiesImplementationFactory = s =>
               new SystemServiceEntities(new DbContextOptionsBuilder<SystemServiceEntities>()
                   .UseSqlServer(Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString,
                                   b => base.DbContextOptionsAction(b, "Senparc.Service")/*从当前程序集读取*/)
                   .Options);
            services.AddScoped(systemServiceEntitiesImplementationFactory);

            services.AddScoped(typeof(ISqlClientFinanceData), typeof(SqlClientFinanceData));
            services.AddScoped(typeof(ISqlBaseFinanceData), typeof(SqlClientFinanceData));

            //预加载 EntitySetKey
            EntitySetKeys.TryLoadSetInfo(typeof(SenparcEntities));
        }


        #endregion
    }
}
