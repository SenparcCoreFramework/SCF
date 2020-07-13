using Microsoft.Extensions.DependencyInjection;
using Senparc.CO2NET.Extensions;
using Senparc.Repository;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Log;
using System;
using System.Threading.Tasks;

namespace Senparc.Service
{
    public class SystemConfigService : BaseClientService<SystemConfig>/*, ISystemConfigService*/
    {
        public SystemConfigService(SystemConfigRepository systemConfigRepo, IServiceProvider serviceProvider)
            : base(systemConfigRepo, serviceProvider)
        {

        }

        public SystemConfig Init(string systemName = null)
        {
            var systemConfig = GetObject(z => true);
            if (systemConfig != null)
            {
                return null;
            }

            systemConfig = new SystemConfig()
            {
                SystemName = systemName ?? "SCF - Template Project"
            };

            SaveObject(systemConfig);

            return systemConfig;
        }

        public override void SaveObject(SystemConfig obj)
        {
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("SystemConfig ���༭��{0}", obj.ToJson());

            //�������
            var fullSystemConfigCache = _serviceProvider.GetService<FullSystemConfigCache>();
            //ʾ��ͬ��������
            using (fullSystemConfigCache.Cache.BeginCacheLock(FullSystemConfigCache.CACHE_KEY, ""))
            {
                fullSystemConfigCache.RemoveCache();
            }
        }

        public override async Task SaveObjectAsync(SystemConfig obj)
        {
            await base.SaveObjectAsync(obj);
            LogUtility.WebLogger.InfoFormat("SystemConfig ���༭��{0}", obj.ToJson());

            //�������
            var fullSystemConfigCache = _serviceProvider.GetService<FullSystemConfigCache>();
            //ʾ��ͬ��������
            using (await fullSystemConfigCache.Cache.BeginCacheLockAsync(FullSystemConfigCache.CACHE_KEY, ""))
            {
                fullSystemConfigCache.RemoveCache();
            }
        }
    }
}

