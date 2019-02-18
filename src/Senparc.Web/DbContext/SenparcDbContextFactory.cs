using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Utilities;
using Senparc.Core.Config;
using Senparc.Core.Models;
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
            builder.UseSqlServer(SenparcDatabaseConfigs.ClientConnectionString, b => b.MigrationsAssembly("Senparc.Web"));
            return new SenparcEntities(builder.Options);
        }
    }
}
