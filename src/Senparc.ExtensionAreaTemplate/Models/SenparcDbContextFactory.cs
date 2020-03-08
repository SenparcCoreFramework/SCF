using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.ExtensionAreaTemplate.Models.DatabaseModel;
using Senparc.Scf.Core.Config;
using Senparc.Scf.XscfBase.Database;
using System;
using System.IO;

namespace Senparc.ExtensionAreaTemplate
{
    /// <summary>
    /// 设计时 DbContext 创建（仅在开发时创建 Code-First 的数据库 Migration 使用，在生产环境不会执行）
    /// </summary>
    public class SenparcDbContextFactory : SenparcDbContextFactoryBase<MySenparcEntities>
    {
        public override string AssemblyName => "Senparc.ExtensionAreaTemplate";

        public override string RootDictionaryPath => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"/*项目根目录*/, "..\\Senparc.Web"/*找到 Web目录，以获取统一的数据库连接字符串配置*/);

        public override MySenparcEntities GetInstance(DbContextOptions<MySenparcEntities> dbContextOptions)
        {
            return new MySenparcEntities(dbContextOptions);
        }
    }
}
