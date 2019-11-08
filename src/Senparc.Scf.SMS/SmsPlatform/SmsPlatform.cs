using System;
using System.Text;

namespace Senparc.Scf.SMS
{
    public interface ISmsPlatform
    {
        string SmsServiceAddress { get; }
        SenparcSmsSetting Setting { get; set; }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="content"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        SmsResult Send(string content, string number);
        /// <summary>
        /// 获取剩余短信数量
        /// </summary>
        /// <returns></returns>
        string GetLastCount();
    }

    public abstract class SmsPlatform : ISmsPlatform
    {
        public virtual string SmsServiceAddress { get; set; }
        public SenparcSmsSetting Setting { get; set; }
        public abstract SmsResult Send(string content, string number);
        public abstract string GetLastCount();

        public SmsPlatform(SmsPlatformType smsPlatformType, string smsServiceAddress, string smsAccountCORPID, string smsAccountName, string smsAccountPassword, string smsAccountSubNumber)
        {
            Setting = new SenparcSmsSetting()
            {
                SmsPlatformType = smsPlatformType,
                SmsAccountCORPID = smsAccountCORPID,
                SmsAccountName = smsAccountName,
                SmsAccountPassword = smsAccountPassword,
                SmsAccountSubNumber = smsAccountSubNumber,
            };
            SmsServiceAddress = smsServiceAddress;
        }


        protected string UrlEncode(string url, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = encoding.GetBytes(url);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(bytes[i], 16));
            }
            return (sb.ToString());
        }
    }
}
