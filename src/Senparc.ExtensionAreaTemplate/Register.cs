using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Trace;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Areas;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Senparc.ExtensionAreaTemplate
{
    public class Register : XscfRegisterBase, IAreaRegister, IXscfRegister
    {
        public Register()
        { }

        #region IAreaRegister 接口

        public string HomeUrl => "/Admin/MyArea/MyHomePage";

        public IMvcBuilder AuthorizeConfig(IMvcBuilder builder)
        {
            builder.AddRazorPagesOptions(options =>
            {
            });

            SenparcTrace.SendCustomLog("系统启动", "完成 Area:MyArea 注册");

            return builder;
        }

        #endregion

        #region IXscfRegister 接口

        public override string Name => "Senparc.Test.MyArea";
        public override string Uid => "1052306E-8C78-4EBF-8CA9-0EB3738350AE";//必须确保全局唯一，生成后必须固定
        public override string Version => "0.0.1";//必须填写版本号

        public override string MenuName => "扩展页面测试模块";
        public override string Icon => "fa fa-dot-circle-o";//参考如：https://colorlib.com/polygon/gentelella/icons.html
        public override string Description => "这是一个示例项目，用于展示如何扩展自己的网页、功能模块，学习之后可以删除。";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new Type[] {
        };

        public override Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }

        public override async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
