using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Scf.XscfBase;
using Senparc.Weixin;
using Senparc.Weixin.Cache.CsRedis;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExtensionCode
{
    
    public class Register : XscfRegisterBase, IXscfRegister
    {
        #region IXscfRegister 接口

        public override string Name => "自定义代码";

        public override string Uid => "FFD88E78-9069-465E-A533-C52E60AEB3FE";

        public override string Version => "";

        public override string MenuName => "";

        public override string Icon => "";

        public override string Description =>
            @"此项目为扩展代码示例项目，不会因为 SCF 框架更新而受影响。
              如果您需要扩展代码，请参考此项目新建项目。本项目请在发布到生产环境之前移除！";

        public override IList<Type> Functions => new Type[] { };

        public override IApplicationBuilder UseXscfModule(IApplicationBuilder app, IRegisterService registerService)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                //从 appsettings.json 获取微信原始注册信息
                var senparcSetting = scope.ServiceProvider.GetService<IOptions<SenparcSetting>>();
                var senparcWeixinSetting = scope.ServiceProvider.GetService<IOptions<SenparcWeixinSetting>>();
                //注册 微信
                registerService //使用 Senparc.Weixin SDK
                    .UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
                    {
                        #region Weixin 设置

                        /* 微信配置开始
                         * 
                         * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
                         */

                        //注册开始

                        #region 微信缓存（按需，必须在 register.UseSenparcWeixin () 之前）

                        //微信的 Redis 缓存，如果不使用则注释掉（开启前必须保证配置有效，否则会抛错）
                        if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
                        {
                            weixinRegister.UseSenparcWeixinCacheCsRedis();
                        }

                        #endregion

                        #region 注册公众号或小程序（按需）

                        //注册公众号（可注册多个）
                        weixinRegister.RegisterMpAccount(senparcWeixinSetting.Value, "SCF")
                                          .RegisterMpAccount("AppId", "Secret", "Senparc_Template")

                        #endregion
                            ;

                        #endregion

                    });
            }

            //基类中会自动注册所有已经添加到数据库的公众号
            return base.UseXscfModule(app);
        }

        #endregion
    }
}
