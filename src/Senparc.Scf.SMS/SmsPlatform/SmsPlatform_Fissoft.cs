using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.SMS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Senparc.Scf.SMS
{
    public class SmsPlatform_Fissoft : SmsPlatform, ISmsPlatform
    {
        public SmsPlatform_Fissoft(string smsServiceAddress, string smsAccountCORPID, string smsAccountName, string smsAccountPassword, string smsAccountSubNumber)
            : base(SmsPlatformType.Fissoft, smsServiceAddress, smsAccountCORPID, smsAccountName, smsAccountPassword, smsAccountSubNumber)
        {

        }

        public override string SmsServiceAddress => "http://api.fissoft.com/pubservice";

        public override SmsResult Send(string content, string number)
        {
            SmsResult smsResult = SmsResult.未知错误;
            try
            {

                Encoding encoding = Encoding.UTF8;
                var wc = new WebClient();
                wc.Encoding = encoding;
                int contentIndex = 0;
                int limitedLetterCount = 65 * 4;

                //组装消息，判断内容长度，太长则分开发
                List<string> messageList = new List<string>();
                while (contentIndex < content.Length)
                {
                    string limitedContent = content.Substring(contentIndex, Math.Min(limitedLetterCount, content.Length - contentIndex));
                    messageList.Add(limitedContent);
                    contentIndex += limitedLetterCount;
                }

                //分批发送
                for (int i = 0; i < messageList.Count; i++)
                {
                    var msg = messageList[i];
                    if (messageList.Count > 1)
                    {
                        msg = $" [{i + 1}/{messageList.Count}]" + msg;
                    }
                    //msg += "【盛派网络】";
                    string encodedLimitedConent = UrlEncode(msg, encoding);



                    var url = string.Format("{0}/SMSSend?OperId={1}&OperPass={2}&Mobiles={3}&Message={4}",
                                    SmsServiceAddress, Setting.SmsAccountCORPID, Setting.SmsAccountPassword, number, encodedLimitedConent);
                    var smsState = wc.DownloadString(url);

                    var fissosoft_SendResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Fissosoft_SendResult>(smsState);
                    if (fissosoft_SendResult.Code == 1)
                    {
                        smsResult = SmsResult.成功;
                    }
                    else
                    {
                        if (!Enum.TryParse(fissosoft_SendResult.Code.ToString(), out smsResult))
                        {
                            smsResult = SmsResult.未知错误;
                        }
                    }
                    Log.LogUtility.SmsLogger.Info($"发送短信成功：{content}，号码：{number}。状态：{smsResult.ToString()}。发送通道：Fissoft");
                    Thread.Sleep(200);
                }
            }
            catch
            {
                Log.LogUtility.SmsLogger.Error($"发送短信失败：{content}，号码：{number}。发送通道：Fissoft");
            }
            finally
            {
            }
            return smsResult;
        }

        public override string GetLastCount()
        {
            var url = string.Format("{0}/SMSAccountInfo?OperId={1}&OperPass={2}",
               SmsServiceAddress, Setting.SmsAccountName, Setting.SmsAccountPassword);

            Encoding encoding = Encoding.UTF8;
            var wc = new WebClient();
            wc.Encoding = encoding;
            var result = wc.DownloadString(url);
            var lastCountResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Fissoft_LastCountResult>(result);
            var value = Newtonsoft.Json.JsonConvert.DeserializeObject<Fissoft_LastCountResult_Value>(lastCountResult.Value);
            return value.SMSNum.ToString();
        }
    }

    /// <summary>
    /// 发送结果
    /// </summary>
    public class Fissosoft_SendResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
    }


    /// <summary>
    /// 获取剩余短信条数结果
    /// 格式：{"Code":1,"Msg":"成功！","Value":"{\"UserName\":\"test\",\"OperId\":\"sms0013\",\"SMSNum\":5846,\"EmergentSMSNum\":10,\"IsEnable\":1}"}
    /// </summary>
    public class Fissoft_LastCountResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Fissoft_LastCountResult的Value类型
    /// </summary>
    public class Fissoft_LastCountResult_Value
    {
        public string UserName { get; set; }
        public string OperId { get; set; }
        public int SMSNum { get; set; }
        public int EmergentSMSNum { get; set; }
        public int IsEnable { get; set; }
    }

}
