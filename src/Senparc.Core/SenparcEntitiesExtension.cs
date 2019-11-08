using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.Core.Models;

namespace Senparc.Core
{
    public static class SenparcEntitiesExtension
    {
        public static IServiceCollection AddSenparcEntitiesDI(this IServiceCollection services)
        {

            //var connectionString = Senparc.Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString;
            //services.AddDbContext<SenparcEntities>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Senparc.Web")));

            /* 参考信息
             *      错误信息：
             *          中文：EnableRetryOnFailure 解决短暂的数据库连接失败
             *          英文：Win32Exception: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond
             *                InvalidOperationException: An exception has been raised that is likely due to a transient failure. Consider enabling transient error resiliency by adding 'EnableRetryOnFailure()' to the 'UseSqlServer' call.
             *      问题解决方案说明：https://www.colabug.com/2329124.html
             */

            services.AddScoped(s => new SenparcEntities(new DbContextOptionsBuilder<SenparcEntities>()
                .UseSqlServer(Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString)
                .Options));
            //#if DEBUG
            //            var connectionString = Senparc.Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString;
            //            services.AddDbContext<SenparcEntities>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Senparc.Web")));
            //#else
            //            services.AddScoped(s => new SenparcEntities(new DbContextOptionsBuilder<SenparcEntities>()
            //                .UseSqlServer(Config.SenparcDatabaseConfigs.ClientConnectionString)
            //                .Options));
            //#endif

            SenparcDI.ResetGlobalIServiceProvider();//清空缓存，下次使用DI会自动重新Build

            return services;
        }
    }
}
