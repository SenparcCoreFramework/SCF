using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;
using Senparc.ExtensionAreaTemplate.Functions;
using Senparc.ExtensionAreaTemplate.Models;
using Senparc.ExtensionAreaTemplate.Models.DatabaseModel;
using Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
using Senparc.ExtensionAreaTemplate.Services;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Senparc.ExtensionAreaTemplate
{
    public class Register : XscfRegisterBase,
        IXscfRegister, //注册 XSCF 基础模块接口（必须）
        IAreaRegister, //注册 XSCF 页面接口（按需选用）
        IXscfDatabase  //注册 XSCF 模块数据库（按需选用）
    {
        public Register()
        { }


        #region IXscfRegister 接口

        public override string Name => "Senparc.ExtensionAreaTemplate";
        public override string Uid => "1052306E-8C78-4EBF-8CA9-0EB3738350AE";//必须确保全局唯一，生成后必须固定
        public override string Version => "0.2.1";//必须填写版本号

        public override string MenuName => "扩展页面测试模块";
        public override string Icon => "fa fa-dot-circle-o";//参考如：https://colorlib.com/polygon/gentelella/icons.html
        public override string Description => "这是一个示例项目，用于展示如何扩展自己的网页、功能模块，学习之后可以删除，也可以以此为基础模板，改成自己的扩展模块（XSCF Modules）。";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new Type[] {
            typeof(DownloadSourceCode)
        };


        public override async Task InstallOrUpdateAsync(IServiceProvider serviceProvider, InstallOrUpdate installOrUpdate)
        {
            MySenparcEntities mySenparcEntities = serviceProvider.GetService<MySenparcEntities>();
            await mySenparcEntities.Database.MigrateAsync().ConfigureAwait(false);//更新数据库

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

        public override async Task UninstallAsync(IServiceProvider serviceProvider, Func<Task> unsinstallFunc)
        {
            //TODO: 删除数据库表（或隐藏）
            //MySenparcEntities mySenparcEntities = serviceProvider.GetService<MySenparcEntities>();
            //await mySenparcEntities.Database.MigrateAsync().ConfigureAwait(false);//更新数据库

            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion

        #region IAreaRegister 接口

        public string HomeUrl => "/Admin/MyApp/MyHomePage";

        public List<AreaPageMenuItem> AareaPageMenuItems => new List<AreaPageMenuItem>() {
             new AreaPageMenuItem(GetAreaHomeUrl(),"首页","fa fa-laptop"),
             new AreaPageMenuItem(GetAreaUrl("/Admin/MyApp/About"),"关于","fa fa-bookmark-o"),
        };

        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
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
            Func<IServiceProvider, MySenparcEntities> implementationFactory = s =>
                new MySenparcEntities(new DbContextOptionsBuilder<MySenparcEntities>()
                   .UseSqlServer(Scf.Core.Config.SenparcDatabaseConfigs.ClientConnectionString,
                                 b => b.MigrationsAssembly("Senparc.ExtensionAreaTemplate"))
                   .Options);
            services.AddScoped(implementationFactory);

            services.AddScoped(typeof(ColorService));

            services.AddScoped(typeof(Color));
            services.AddScoped(typeof(ColorDto));

            EntitySetKeys.GetEntitySetKeys(typeof(MySenparcEntities));//注册当前数据库的对象（必须）

            return base.AddXscfModule(services);
        }

        #endregion

        #region IXscfDatabase 接口

        public string UniquePrefix => DATABASE_PREFIX;

        /// <summary>
        /// 数据库前缀
        /// </summary>
        public const string DATABASE_PREFIX = "AreaTemplate_";


        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaTemplate_ColorConfigurationMapping());
        }

        #endregion
    }
}
