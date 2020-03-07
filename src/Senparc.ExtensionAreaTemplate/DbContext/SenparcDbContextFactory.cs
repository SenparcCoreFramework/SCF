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
    /// 设计时 DbContext 创建
    /// </summary>
    public class SenparcDbContextFactory : SenparcDbContextFactoryBase<MySenparcEntities>
    {
        public override string AssemblyName => "Senparc.ExtensionAreaTemplate";

        public override MySenparcEntities GetInstance(DbContextOptions<MySenparcEntities> dbContextOptions)
        {
            return new MySenparcEntities(dbContextOptions);
        }
    }
}
