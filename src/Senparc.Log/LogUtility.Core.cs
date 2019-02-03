using log4net;

namespace Senparc.Log
{
    public static partial class LogUtility
    {
        public static object LogLock = new object();

        public static ILog WebLogger => GetLogger("WebLoggerRepository");
        public static ILog EmailLogger => GetLogger("EmailLoggerRepository");
        public static ILog SystemLogger => GetLogger("SystemLoggerRepository");
        public static ILog ContentLogger => GetLogger("ContentRepository");
        public static ILog Materia => GetLogger("MateriaRepository");
        public static ILog Menu => GetLogger("MenuRepository");
        public static ILog QiNiu => GetLogger("QiNiuService");
        public static ILog App => GetLogger("AppService");

        public static ILog AccountPayLog => GetLogger("AccountPayLog");
        public static ILog Pay => GetLogger("Pay");

        public static ILog ApmEndingData => GetLogger("ApmEndingData");
        public static ILog ApmEndingDataStat => GetLogger("ApmEndingDataStat");

        public static ILog DeveloperApi => GetLogger("DeveloperApiService");
        public static ILog Developer => GetLogger("DeveloperService");

        public static log4net.ILog P2pApi => GetLogger("P2pApi");


        public static ILog Cache => GetLogger("CacheRepository");
        public static ILog SmsLogger => GetLogger("SmsLoggerRepository");
        public static ILog Account => GetLogger("AccountRepository");
        public static ILog OperationQueue => GetLogger("OperationQueueRepository");

        public static ILog WeixinOAuth => GetLogger("WeixinOAuth");
        public static ILog OpenOAuth => GetLogger("OpenOAuth");


        public static ILog AdminUserInfo => GetLogger("AdminUserInfoRepository");

        public static ILog Weixin => GetLogger("Weixin");
        public static ILog Neural => GetLogger("NeuralRepository");
        public static ILog NeuralApp => GetLogger("NeuralAppRepository");

        public static ILog Comment => GetLogger("CommentRepository");

        public static ILog Report => GetLogger("ReportRepository");

        public static ILog TrackPageLoadPerformance => GetLogger("TrackPageLoadPerformance");
        public static ILog Reply => GetLogger("ReplyRepository");


        public static int Int { get; set; }
        //新建的领域可以在这里继续添加

        public static ILog GetLogger(string name)
        {
            lock (LogLock)
            {
                //var appenders = LogManager.GetRepository().GetAppenders();
                //if (appenders.Count() == 0)
                //{
                //    //没有载入log成功
                //    return log4net.LogManager.GetLogger(name);
                //}

                //var fileAppender = appenders.First(appender => appender.Name == "SysLogAppender") as RollingFileAppender;
                //fileAppender.File = "App_Data/";
                //fileAppender.ActivateOptions();
                //var repository = LogManager.GetRepository(name) ?? LogManager.CreateRepository(name);
                return LogManager.GetLogger("NETCoreRepository", name);
            }
        }
    }
}
