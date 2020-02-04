using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.Utilities;
using Senparc.Scf.Core.Config;
using Senparc.Scf.SMS;
using Senparc.Web.Hubs;
using Senparc.Weixin;
using Senparc.Weixin.Cache.CsRedis;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.Open;
using Senparc.Weixin.Open.ComponentAPIs;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Senparc.CO2NET.AspNet;

namespace Senparc.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
            //读取Log配置文件
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var cache = services.BuildServiceProvider().GetService<IMemoryCache>();//测试成功
            services
                .Configure<SenparcWeixinSetting>(Configuration.GetSection("SenparcWeixinSetting"))
                .Configure<SenparcSmsSetting>(Configuration.GetSection("SenparcSmsSetting"))//TODO：让SMS模块进行注册
                ;


            //启用以下代码强制使用 https 访问
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 443;
            //});


            //添加 SenparcCoreSetting 配置文件（内容可以根据需要对应修改）
            //注册数据库客户端连接

            //添加（注册） Scf 服务（重要，必须！）
            services.AddScfServices(Configuration, env, CompatibilityVersion.Version_3_0);

            services.AddSenparcWeixinServices(Configuration); //Senparc.Weixin 注册（已自带 Senparc.CO2NET 全局注册）
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<SenparcSetting> senparcSetting,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting, IHubContext<ReloadPageHub> hubContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/node_modules")
            });



            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // 启动 CO2NET 全局注册，必须！
            // 关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
            var registerService = app
                //全局注册
                .UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
                {
                    #region CO2NET 全局配置

                    #region 全局缓存配置（按需）

                    #region 配置和使用 Redis

                    //配置全局使用Redis缓存（按需，独立）
                    if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
                    {
                        /* 说明：
                         * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                        /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                         */
                        Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption(redisConfigurationStr);

                        //以下会立即将全局缓存设置为 Redis
                        Senparc.CO2NET.Cache.CsRedis.Register.UseKeyValueRedisNow(); //键值对缓存策略（推荐）
                                                                                   //Senparc.CO2NET.Cache.Redis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                        //也可以通过以下方式自定义当前需要启用的缓存策略
                        //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                        //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet
                    }
                    //如果这里不进行Redis缓存启用，则目前还是默认使用内存缓存 

                    #endregion

                    #endregion

                    #region 注册日志（按需，建议）

                    globalRegister.RegisterTraceLog(ConfigTraceLog); //配置TraceLog

                    #endregion


                    #endregion
                })
                //使用 Senparc.Weixin SDK
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

                        //注册多个公众号或小程序（可注册多个）
                        //.RegisterWxOpenAccount(senparcWeixinSetting.Value, "【盛派网络小助手】小程序")
                        //注册第三方平台（可注册多个）

                    #region 注册第三方平台

                        .RegisterOpenComponent(senparcWeixinSetting.Value,
                          //getComponentVerifyTicketFunc
                          async componentAppId =>
                          {

                              var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/OpenTicket"));
                              if (!Directory.Exists(dir))
                              {
                                  Directory.CreateDirectory(dir);
                              }

                              var file = Path.Combine(dir, string.Format("{0}.txt", componentAppId));
                              using (var fs = new FileStream(file, FileMode.Open))
                              {
                                  using (var sr = new StreamReader(fs))
                                  {
                                      var ticket = await sr.ReadToEndAsync();
                                      return ticket;
                                  }
                              }
                          },

                           //getAuthorizerRefreshTokenFunc
                           async (componentAppId, auhtorizerId) =>
                           {
                               var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                               if (!Directory.Exists(dir))
                               {
                                   Directory.CreateDirectory(dir);
                               }

                               var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                               if (!System.IO.File.Exists(file))
                               {
                                   return null;
                               }

                               using (Stream fs = new FileStream(file, FileMode.Open))
                               {
                                   var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                   var result = (RefreshAuthorizerTokenResult)binFormat.Deserialize(fs);
                                   return result.authorizer_refresh_token;
                               }
                           },

                             //authorizerTokenRefreshedFunc
                             (componentAppId, auhtorizerId, refreshResult) =>
                             {
                                 var dir = Path.Combine(ServerUtility.ContentRootMapPath("~/App_Data/AuthorizerInfo/" + componentAppId));
                                 if (!Directory.Exists(dir))
                                 {
                                     Directory.CreateDirectory(dir);
                                 }

                                 var file = Path.Combine(dir, string.Format("{0}.bin", auhtorizerId));
                                 using (Stream fs = new FileStream(file, FileMode.Create))
                                 {
                                     var binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                                     binFormat.Serialize(fs, refreshResult);
                                     fs.Flush();
                                 }
                             }, "【盛派网络】开放平台")

                    #endregion

                        //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                        //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers

                    #endregion

                    #region 注册微信支付（按需）

                        //注册最新微信支付版本（V3）（可注册多个）
                        .RegisterTenpayV3(senparcWeixinSetting.Value, "SCF") //记录到同一个 SenparcWeixinSettingItem 对象中

                    #endregion

                    ;

                    #endregion

                });

            #region .NET Core默认不支持GB2312

            //http://www.mamicode.com/info-detail-2225481.html
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            #endregion

            #region Senparc.Core 设置

            //用于解决HttpContext.Connection.RemoteIpAddress为null的问题
            //https://stackoverflow.com/questions/35441521/remoteipaddress-is-always-null
            app.UseHttpMethodOverride(new HttpMethodOverrideOptions
            {
                //FormFieldName = "X-Http-Method-Override"//此为默认值
            });

            app.UseSenparcMvcDI();

            //Senparc.Scf.Core.Config.SiteConfig.SenparcCoreSetting = senparcCoreSetting.Value;//网站设置

            //提供网站根目录
            if (env.ContentRootPath != null)
            {
                Senparc.Scf.Core.Config.SiteConfig.ApplicationPath = env.ContentRootPath;
                Senparc.Scf.Core.Config.SiteConfig.WebRootPath = env.WebRootPath;
            }

            #endregion

            #region 异步线程

            {
                ////APM Ending 数据统计
                //var utility = new APMNeuralDataThreadUtility();
                //Thread thread = new Thread(utility.Run) { Name = "APMNeuralDataThread" };
                //SiteConfig.AsynThread.Add(thread.Name, thread);
            }

            SiteConfig.AsynThread.Values.ToList().ForEach(z =>
            {
                z.IsBackground = true;
                z.Start();
            }); //全部运行 

            #endregion
        }

        /// <summary>
        /// 配置微信跟踪日志
        /// </summary>
        private void ConfigTraceLog()
        {
            //这里设为Debug状态时，/App_Data/WeixinTraceLog/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭

            //如果全局的IsDebug（Senparc.CO2NET.Config.IsDebug）为false，此处可以单独设置true，否则自动为true
            CO2NET.Trace.SenparcTrace.SendCustomLog("系统日志",
                "SenparcCoreFramework 系统启动"); //只在Senparc.Weixin.Config.IsDebug = true的情况下生效

            //全局自定义日志记录回调
            CO2NET.Trace.SenparcTrace.OnLogFunc = () =>
            {
                //加入每次触发Log后需要执行的代码
            };

            //当发生基于WeixinException的异常时触发
            WeixinTrace.OnWeixinExceptionFunc = ex =>
            {
                //加入每次触发WeixinExceptionLog后需要执行的代码

                //发送模板消息给管理员
                //var eventService = new Senparc.Weixin.MP.Sample.CommonService.EventService();
                //eventService.ConfigOnWeixinExceptionFunc(ex);
            };
        }

        /// <summary>
        /// 判断当前配置是否满足使用 Redis（根据是否已经修改了默认配置字符串判断）
        /// </summary>
        /// <param name="senparcSetting"></param>
        /// <returns></returns>
        private bool UseRedis(SenparcSetting senparcSetting, out string redisConfigurationStr)
        {
            redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*默认值，不启用*/;
            return useRedis;
        }
    }


    public static class PhysicalFileAppBuilderExtensions
    {
        private static readonly PhysicalFileProvider _fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

        /// <summary>
        /// 检测文件变化
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hubContext"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePhysicalFile(this IApplicationBuilder app, IHubContext<ReloadPageHub> hubContext)
        {
            RegisterPhysical(hubContext);
            return app;
        }

        /// <summary>
        /// 注册检查
        /// </summary>
        /// <param name="hubContext"></param>
        public static void RegisterPhysical(IHubContext<ReloadPageHub> hubContext)
        {
            Task.Run(() =>
            {
                var tcs = new TaskCompletionSource<object>();
                while (true)
                {
                    PhysicalFileAsync(hubContext).GetAwaiter().GetResult();
                }
            });
        }

        /// <summary>
        /// 检查文件变化
        /// </summary>
        /// <param name="hubContext"></param>
        /// <returns></returns>
        private static async Task PhysicalFileAsync(IHubContext<ReloadPageHub> hubContext)
        {
            var jsToken = _fileProvider.Watch("wwwroot/**/*.js");
            var cssToken = _fileProvider.Watch("wwwroot/**/*.css");
            var cshtmlToken = _fileProvider.Watch("**/*.cshtml");
            var tcs = new TaskCompletionSource<object>();
            //TODO:如果考虑效率问题可以不使用while(true)，可以选中在RegisterChangeCallback 内部再次调用fileProvider.Watch
            jsToken.RegisterChangeCallback(state =>
            {
                ((TaskCompletionSource<object>)state).TrySetResult(null);
                hubContext.Clients.All.SendAsync("ReloadPage", "js文件发生变化");
            }, tcs);
            cssToken.RegisterChangeCallback(state =>
            {
                ((TaskCompletionSource<object>)state).TrySetResult(null);
                hubContext.Clients.All.SendAsync("ReloadPage", "css文件发生变化");
            }, tcs);
            cshtmlToken.RegisterChangeCallback(state =>
            {
                ((TaskCompletionSource<object>)state).TrySetResult(null);
                hubContext.Clients.All.SendAsync("ReloadPage", "cshtml文件发生变化");
            }, tcs);
            await tcs.Task.ConfigureAwait(false);
        }
    }
}