using System;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Log;
using Senparc.Scf.Repository;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Utility;
using Senparc.CO2NET;
using Senparc.Core.Models;
using Senparc.Repository;
using Senparc.Core.Cache;

namespace Senparc.Service
{
    //public interface ISystemConfigService : IBaseClientService<SystemConfig>
    //{
    //    string GetRuningDatabasePath();
    //    string BackupDatabase();
    //    void RestoreDatabase(string fileName);
    //    void DeleteBackupDatabase(string fileName, bool deleteAllBefore);
    //    void RecycleAppPool();
    //}

    public class SystemConfigService : ClientServiceBase<SystemConfig>/*, ISystemConfigService*/
    {
        public SystemConfigService(SystemConfigRepository systemConfigRepo)
            : base(systemConfigRepo)
        {

        }

        public SystemConfig Init()
        {
           var systemConfig = GetObject(z => true);
            if (systemConfig!=null)
            {
                return null;
            }

            systemConfig = new SystemConfig() {
                SystemName = "SCF - Template Project"
            };

            SaveObject(systemConfig);

            return systemConfig;
        }

        public override void SaveObject(SystemConfig obj)
        {
            LogUtility.SystemLogger.Info("系统信息被编辑");

            base.SaveObject(obj);

            //删除缓存
            var systemConfigCache = SenparcDI.GetService<FullSystemConfigCache>();
            systemConfigCache.RemoveCache();
        }

        public string GetRuningDatabasePath()
        {
            var dbPath = "~/App_Data/#SenparcCRM.config";
            return dbPath;
        }

        public string BackupDatabase()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd-HH-mm");//分钟
            return timeStamp;
        }

        public void RecycleAppPool()
        {
            //string webConfigPath = HttpContext.Current.Server.MapPath("~/Web.config");
            //System.IO.File.SetLastWriteTimeUtc(webConfigPath, DateTime.UtcNow);
        }

        public override void DeleteObject(SystemConfig obj)
        {
            throw new Exception("系统信息不能被删除！");
        }
    }
}

