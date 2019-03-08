
using Senparc.Scf.SMS;

namespace Senparc.Scf.SMS
{
    public static class SmsPlatformFactory
    {
        public static ISmsPlatform GetSmsPlateform(string smsAccountCorpid, string smsAccountName,
            string smsAccountPassword, string smsAccountSubNumber, SmsPlatformType smsPlatformType = SmsPlatformType.JunMei)
        {
            switch (smsPlatformType)
            {
                case SmsPlatformType.Fissoft:
                    return new SmsPlatform_Fissoft(null, smsAccountCorpid, smsAccountName, smsAccountPassword, smsAccountSubNumber);
                default:
                    return new SmsPlatform_JunMei(null, smsAccountCorpid, smsAccountName, smsAccountPassword, smsAccountSubNumber);
            }
        }
    }
}
