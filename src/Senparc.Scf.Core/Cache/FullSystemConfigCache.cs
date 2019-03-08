using System.Linq;
using Microsoft.AspNetCore.Http;
using Senparc.CO2NET;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Exceptions;
using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Core.Cache
{
    //public interface IFullSystemConfigCache : IBaseCache<FullSystemConfig>
    //{

    //}

    [AutoDIType(DILifecycleType.Singleton)]
    public class FullSystemConfigCache : BaseCache<FullSystemConfig>/*, IFullSystemConfigCache*/
    {
        public const string CACHE_KEY = "FullSystemConfigCache";

        public FullSystemConfigCache(ISqlClientFinanceData db)
            : base(CACHE_KEY, db)
        {
            base.TimeOut = 1440;
        }

        public override FullSystemConfig Update()
        {
            var systemConfig = base._db.DataContext.SystemConfigs.FirstOrDefault();
            FullSystemConfig fullSystemConfig = null;
            if (systemConfig != null)
            {
                fullSystemConfig = FullSystemConfig.CreateEntity<FullSystemConfig>(systemConfig);
            }
            else
            {
                string hostName = null;
                try
                {
                    var httpContextAccessor = SenparcDI.GetService<IHttpContextAccessor>();
                    var httpContext = httpContextAccessor.HttpContext;
                    var urlData = httpContext.Request;
                    var scheme = urlData.Scheme;//协议
                    var host = urlData.Host.Host;//主机名（不带端口）
                    var port = urlData.Host.Port ?? -1;//端口（因为从.NET Framework移植，因此不直接使用urlData.Host）
                    string portSetting = null;//Url中的端口部分
                    string schemeUpper = scheme.ToUpper();//协议（大写）
                    string baseUrl = httpContext.Request.PathBase;//子站点应用路径

                    if (port == -1 || //这个条件只有在 .net core 中， Host.Port == null 的情况下才会发生
               (schemeUpper == "HTTP" && port == 80) ||
               (schemeUpper == "HTTPS" && port == 443))
                    {
                        portSetting = "";//使用默认值
                    }
                    else
                    {
                        portSetting = ":" + port;//添加端口
                    }

                    hostName = $"{scheme}://{host}{portSetting}";

                }
                catch
                {
                }
                throw new SCFExceptionBase($"SCF 系统未初始化，请先执行 {hostName}/Install 进行数据初始化");
            }

            base.SetData(fullSystemConfig, base.TimeOut, null);
            return base.Data;
        }
    }
}
