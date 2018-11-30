using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Senparc.Core.Config;
using Senparc.Core.Models;

namespace Senparc.Web
{
    /// <summary>
    /// 设计时 DbContext 创建
    /// </summary>
    public class SenparcDbContextFactory : IDesignTimeDbContextFactory<SenparcEntities>
    {
        public SenparcEntities CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SenparcEntities>();
            builder.UseSqlServer(SenparcDatabaseConfigs.ClientConnectionString, b => b.MigrationsAssembly("Senparc.Web"));
            return new SenparcEntities(builder.Options);
        }
    }
}
