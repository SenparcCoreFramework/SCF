using log4net;

namespace Senparc.Scf.Log
{
    public static partial class LogUtility
    {
        public static object LogLock = new object();

        public static ILog WebLogger => GetLogger("WebLogger");
        public static ILog Cache => GetLogger("Cache");
        public static ILog OperationQueue => GetLogger("OperationQueue");
        public static ILog EmailLogger => GetLogger("EmailLogger");
        public static ILog SystemLogger => GetLogger("SystemLogger");
        public static ILog AccountPayLog => GetLogger("AccountPayLog");
        public static ILog SmsLogger => GetLogger("SmsLogger");
        public static ILog Account => GetLogger("Account");
        public static ILog AdminUserInfo => GetLogger("AdminUserInfo");
        public static ILog WeixinOAuth => GetLogger("WeixinOAuth");
        public static ILog TrackPageLoadPerformance => GetLogger("TrackPageLoadPerformance");
        public static ILog Weixin => GetLogger("Weixin");


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
