using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Xscf.ExtensionAreaTemplate.Functions;
using Senparc.Xscf.ExtensionAreaTemplate.Models;
using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel;
using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
using Senparc.Xscf.ExtensionAreaTemplate.Services;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Senparc.Scf.Core.Config;

namespace Senparc.Xscf.ExtensionAreaTemplate
{
    public class Register : XscfRegisterBase,
        IXscfRegister, //注册 XSCF 基础模块接口（必须）
        IAreaRegister, //注册 XSCF 页面接口（按需选用）
        IXscfDatabase,  //注册 XSCF 模块数据库（按需选用）
        IXscfRazorRuntimeCompilation  //需要使用 RazorRuntimeCompilation，在开发环境下实时更新 Razor Page
    {
        public Register()
        { }


        #region IXscfRegister 接口

        public override string Name => "Senparc.Xscf.ExtensionAreaTemplate";
        public override string Uid => "1052306E-8C78-4EBF-8CA9-0EB3738350AE";//必须确保全局唯一，生成后必须固定
        public override string Version => "0.2.2";//必须填写版本号

        public override string MenuName => "扩展页面测试模块";
        public override string Icon => "fa fa-dot-circle-o";//参考如：https://colorlib.com/polygon/gentelella/icons.html
        public override string Description => "这是一个示例项目，用于展示如何扩展自己的网页、功能模块，学习之后可以删除，也可以以此为基础模板，改成自己的扩展模块（XSCF Modules）。";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new Type[] {
            typeof(DownloadSourceCode)
        };


        /// <summary>
        /// 安装或更新过程需要执行的业务
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="installOrUpdate"></param>
        /// <returns></returns>
        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            //更新数据库
            await base.MigrateDatabaseAsync<MySenparcEntities>(serviceProvider);

            switch (installOrUpdate)
            {
                case InstallOrUpdate.Install:
                    //新安装
                    var colorService = serviceProvider.GetService<ColorService>();
                    var color = colorService.GetObject(z => true);
                    if (color == null)//如果是纯第一次安装，理论上不会有残留数据
                    {
                        //创建默认颜色
                        ColorDto colorDto = await colorService.CreateNewColor().ConfigureAwait(false);
                    }
                    break;
                case InstallOrUpdate.Update:
                    //更新
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// 删除模块时需要执行的业务
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="unsinstallFunc"></param>
        /// <returns></returns>
        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            MySenparcEntities mySenparcEntities = serviceProvider.GetService<MySenparcEntities>();
            
            //指定需要删除的数据实体

            //注意：这里作为演示，删除了所有的表，实际操作过程中，请谨慎操作，并且按照删除顺序对实体进行排序！
            var dropTableKeys = EntitySetKeys.GetEntitySetInfo(this.XscfDatabaseDbContextType).Keys.ToArray();
            await base.DropTablesAsync(serviceProvider, mySenparcEntities, dropTableKeys);

            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion

        #region IAreaRegister 接口

        public string HomeUrl => "/Admin/MyApp/MyHomePage";

        public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
             new AreaPageMenuItem(GetAreaHomeUrl(),"首页","fa fa-laptop"),
             new AreaPageMenuItem(GetAreaUrl("/Admin/MyApp/About"),"关于","fa fa-bookmark-o"),
        };

        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder, IWebHostEnvironment env)
        {
            builder.AddRazorPagesOptions(options =>
            {
                //此处可配置页面权限
            });

            SenparcTrace.SendCustomLog("系统启动", "完成 Area:MyApp 注册");

            return builder;
        }

        public override IServiceCollection AddXscfModule(IServiceCollection services)
        {
            //任何需要注册的对象
            return base.AddXscfModule(services);
        }

        #endregion

        #region IXscfDatabase 接口

        /// <summary>
        /// 数据库前缀
        /// </summary>
        public const string DATABASE_PREFIX = "AreaTemplate_";

        /// <summary>
        /// 数据库前缀
        /// </summary>
        public string DatabaseUniquePrefix => DATABASE_PREFIX;
        /// <summary>
        /// 设置 XscfSenparcEntities 类型
        /// </summary>
        public Type XscfDatabaseDbContextType => typeof(MySenparcEntities);


        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaTemplate_ColorConfigurationMapping());
        }

        public void AddXscfDatabaseModule(IServiceCollection services)
        {
            services.AddScoped(typeof(Color));
            services.AddScoped(typeof(ColorDto));
            services.AddScoped(typeof(ColorService));
        }

        #endregion

        #region IXscfRazorRuntimeCompilation 接口
        public string LibraryPath => Path.GetFullPath(Path.Combine(SiteConfig.WebRootPath, "..", "Senparc.Xscf.ExtensionAreaTemplate"));
        #endregion
    }
}
