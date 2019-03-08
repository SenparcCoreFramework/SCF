using System;
using System.Xml.Linq;

namespace Senparc.Scf.Core.Email
{
    using Senparc.Scf.Core.Enums;

    public static class AutoSendLogEmail
    {
        public static void SendLogEmail(Exception e)
        {
            XDocument doc = new XDocument();
            doc.Root.Add(new XElement("ErrorLogo",
                            new XElement("Time", DateTime.Now),
                            new XElement("Exception Message", e.Message),
                            new XElement("InnerException", e.InnerException),
                            new XElement("StackTrace", e.StackTrace)
                            ));
            SendEmail sendExceptionEmail = new SendEmail(SendEmailType.CustomEmail);
            SendEmailParameter_CustomEmail sendEmailParam = new SendEmailParameter_CustomEmail("zsu@senparc.com", "[后台运行]", "Senparc运行错误", doc.ToString());
            sendExceptionEmail.Send(sendEmailParam, true, true, EmailAccountType.Default);
        }
    }
}