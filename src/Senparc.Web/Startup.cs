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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.CO2NET.Utilities;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Models;
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
            //Senparc.Weixin 注册（已自带 Senparc.CO2NET 全局注册）
            services.AddSenparcWeixinServices(Configuration);
            services.Configure<SenparcWeixinSetting>(Configuration.GetSection("SenparcWeixinSetting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<SenparcCoreSetting> senparcCoreSetting,
            IOptions<SenparcSetting> senparcSetting,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting,
            IHubContext<ReloadPageHub> hubContextd)
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

            //Use SCF
            app.UseScf(env, senparcCoreSetting, senparcSetting, senparcWeixinSetting);
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