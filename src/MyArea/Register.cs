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

namespace MyArea
{
    public class Register : IAreaRegister, IXscfRegister
    {
        public Register()
        { }

        #region IAreaRegister 接口

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

        public string Name => "Senparc.Test.MyArea";
        public string Uid => "1052306E-8C78-4EBF-8CA9-0EB3738350AE";//必须确保全局唯一，生成后必须固定
        public string Version => "0.0.1";//必须填写版本号

        public string MenuName => "扩展页面测试模块";
        public string Icon => "fa fa-dot-circle-o";//参考如：https://colorlib.com/polygon/gentelella/icons.html
        public string Description => "这是一个示例项目，用于展示如何扩展自己的网页、功能模块，学习之后可以删除。";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public IList<Type> Functions => new Type[] {
        };

        public virtual Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }

        public virtual async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
