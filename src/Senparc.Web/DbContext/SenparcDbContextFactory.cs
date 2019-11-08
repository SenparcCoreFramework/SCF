using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Utilities;
using Senparc.Core.Models;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Senparc.Web
{
    /// <summary>
    /// 设计时 DbContext 创建
    /// </summary>
    public class SenparcDbContextFactory : IDesignTimeDbContextFactory<SenparcEntities>
    {
        public SenparcEntities CreateDbContext(string[] args)
        {
            //修复 https://github.com/SenparcCoreFramework/SCF/issues/13 发现的问题（在非Web环境下无法得到网站根目录路径）
            IRegisterService register = RegisterService.Start(new SenparcSetting());
            CO2NET.Config.RootDictionaryPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"); //

            var builder = new DbContextOptionsBuilder<SenparcEntities>();

            //如果运行 Add-Migration 命令，并且获取不到正确的网站根目录，此处可能无法自动获取到连接字符串（上述#13问题），
            //也可通过下面已经注释的的提供默认值方式解决（不推荐）
            var sqlConnection = SenparcDatabaseConfigs.ClientConnectionString; //?? "Server=.\\;Database=SCF;Trusted_Connection=True;integrated security=True;";
            builder.UseSqlServer(sqlConnection, b => b.MigrationsAssembly("Senparc.Web"));
            return new SenparcEntities(builder.Options);
        }
    }
}
