using Senparc.Core.Enums;

namespace Senparc.SMS
{
    public class SenparcSmsSetting
    {
        public string SmsAccountCORPID { get; set; }
        public string SmsAccountName { get; set; }
        public string SmsAccountSubNumber { get; set; }
        public string SmsAccountPassword { get; set; }
        public SmsPlatformType SmsPlatformType { get; set; }

    }
}
