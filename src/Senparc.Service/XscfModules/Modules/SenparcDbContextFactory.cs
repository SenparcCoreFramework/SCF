using Senparc.Core.Models;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Senparc.Service.Modules
{
    ///// <summary>
    ///// 设计时 DbContext 创建（仅在开发时创建 Code-First 的数据库 Migration 使用，在生产环境不会执行）
    ///// </summary>
    public class SenparcDbContextFactory : SenparcDesignTimeDbContextFactoryBase<SenparcEntities, Register>
    {
        /// <summary>
        /// 用于寻找 App_Data 文件夹，从而找到数据库连接字符串配置信息
        /// </summary>
        public override string RootDictionaryPath => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "..\\Senparc.Web"/* Debug模式下项目根目录 */);
    }
}