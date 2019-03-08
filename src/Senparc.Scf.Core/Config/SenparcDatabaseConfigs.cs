using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;
using Senparc.Scf.Log;
using System;
using System.Collections.Generic;

namespace Senparc.Scf.Core.Config
{
    public static class SenparcDatabaseConfigs
    {
        public const string SENPARC_CONFIG_KEY = "__SENPARC_DATABASE_CONFIG_KEY";
        public static Dictionary<string, SenparcConfig> Configs
        {
            get
            {
                Func<Dictionary<string, SenparcConfig>> func = () =>
                {
                    Dictionary<string, SenparcConfig> configs = new Dictionary<string, SenparcConfig>();
                    try
                    {
                        XmlDataContext xmlCtx = new XmlDataContext(SiteConfig.SenparcConfigDirctory);
                        var list = xmlCtx.GetXmlList<SenparcConfig>();
                        list.ForEach(z => configs.Add(z.Name, z));
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

                if (SenparcDatabaseConfigs.Configs != null && SenparcDatabaseConfigs.Configs.ContainsKey("Client"))
                {
                    //根据数据库类型不同，区分输出连接字符串。
                    //string provider = "System.Data.SqlClient";
                    //return string.Format(@"metadata=res://*/Models.Sprent.csdl|res://*/Models.Sprent.ssdl|res://*/Models.Sprent.msl;provider={0};provider connection string='{1}';"
                    //    , provider, HandleIdeaConfigs.Config.ConnectionString);
                    return SenparcDatabaseConfigs.Configs["Client"].ConnectionStringFull;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
