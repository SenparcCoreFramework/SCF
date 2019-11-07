using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Exceptions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;
using Senparc.Scf.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Senparc.Scf.Core.Config
{
    public static class SenparcDatabaseConfigs
    {
        public const string SENPARC_CONFIG_KEY = "__SENPARC_DATABASE_CONFIG_KEY";
        public static ConcurrentDictionary<string, SenparcConfig> Configs
        {
            get
            {
                Func<ConcurrentDictionary<string, SenparcConfig>> func = () =>
                {
                    ConcurrentDictionary<string, SenparcConfig> configs = new ConcurrentDictionary<string, SenparcConfig>();
                    try
                    {
                        XmlDataContext xmlCtx = new XmlDataContext(SiteConfig.SenparcConfigDirctory);
                        var list = xmlCtx.GetXmlList<SenparcConfig>();
                        list.ForEach(z => configs[z.Name] = z);
                    }
                    catch (Exception e)
                    {
                        LogUtility.WebLogger.ErrorFormat("SenparcConfigs.Configs读取错误：" + e.Message, e);
                    }
                    return configs;
                };
                //ICommonDataCache<SenparcConfig> dataCache = new CommonDataCache<SenparcConfig>(SENPARC_CONFIG_KEY, func);
                //return dataCache.Data;

                var cacheData = MethodCache.GetMethodCache(SENPARC_CONFIG_KEY, func, 60 * 999);
                return cacheData;
            }
        }

        /// <summary>
        /// 主站客户数据库SQL连接字符串(for EF)
        /// </summary>
        public static string ClientConnectionString
        {
            get
            {
                var databaseName = Config.SiteConfig.SenparcCoreSetting.DatabaseName ?? "Client";
                if (SenparcDatabaseConfigs.Configs != null && SenparcDatabaseConfigs.Configs.ContainsKey(databaseName))
                {
                    //根据数据库类型不同，区分输出连接字符串。
                    //string provider = "System.Data.SqlClient";
                    //return string.Format(@"metadata=res://*/Models.Sprent.csdl|res://*/Models.Sprent.ssdl|res://*/Models.Sprent.msl;provider={0};provider connection string='{1}';"
                    //    , provider, HandleIdeaConfigs.Config.ConnectionString);
                    return SenparcDatabaseConfigs.Configs[databaseName].ConnectionStringFull;
                }
                else
                {
                    throw new SCFExceptionBase($"无法找到数据库配置：{databaseName}，请在 SenparcConfig.config 中进行配置");
                }
            }
        }
    }
}
