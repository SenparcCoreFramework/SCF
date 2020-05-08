using Microsoft.Extensions.DependencyInjection;
using Senparc.Repository;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Log;
using System;

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
    }
}

