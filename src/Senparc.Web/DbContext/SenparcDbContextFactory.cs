using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Models;

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

            //如果运行 Add-Migration 命令，此处可能无法自动获取到连接字符串
            var sqlConnection = SenparcDatabaseConfigs.ClientConnectionString ?? "Server=.\\;Database=SCF;Trusted_Connection=True;integrated security=True;";
            builder.UseSqlServer(sqlConnection, b => b.MigrationsAssembly("Senparc.Web"));
            return new SenparcEntities(builder.Options);
        }
    }
}
