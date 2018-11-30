using System.IO;
using log4net;
using log4net.Config;

namespace Senparc.SMS.Tests
{
    public class BaseSMSTest
    {
        public BaseSMSTest()
        {
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }
    }
}
