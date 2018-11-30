using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Core.Models;

namespace Senparc.Core
{
    public static class SenparcEntitiesExtension
    {
        public static IServiceCollection AddSenparcEntitiesDI(this IServiceCollection services)
        {

            //var connectionString = Senparc.Core.Config.SenparcDatabaseConfigs.ClientConnectionString;
            //services.AddDbContext<SenparcEntities>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Senparc.Web")));

            services.AddScoped(s => new SenparcEntities(new DbContextOptionsBuilder<SenparcEntities>()
                .UseSqlServer(Config.SenparcDatabaseConfigs.ClientConnectionString)
                .Options));
            //#if DEBUG
            //            var connectionString = Senparc.Core.Config.SenparcDatabaseConfigs.ClientConnectionString;
            //            services.AddDbContext<SenparcEntities>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Senparc.Web")));
            //#else
            //            services.AddScoped(s => new SenparcEntities(new DbContextOptionsBuilder<SenparcEntities>()
            //                .UseSqlServer(Config.SenparcDatabaseConfigs.ClientConnectionString)
            //                .Options));
            //#endif

            return services;
        }
    }
}
