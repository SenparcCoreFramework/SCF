using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Log;
using Senparc.Scf.SMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Senparc.Scf.SMS
{
    public class SmsPlatform_JunMei : SmsPlatform, ISmsPlatform
    {
        public SmsPlatform_JunMei(string smsServiceAddress, string smsAccountCORPID, string smsAccountName, string smsAccountPassword, string smsAccountSubNumber)
            : base(SmsPlatformType.JunMei, smsServiceAddress, smsAccountCORPID, smsAccountName, smsAccountPassword, smsAccountSubNumber)
        {

        }

        public override string SmsServiceAddress => "http://120.77.14.55:8888/sms.aspx";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public override SmsResult Send(string content, string number)
        {
            SmsResult smsResult = SmsResult.未知错误;
            try
            {
                Encoding encoding = Encoding.UTF8;
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
                    string encodedLimitedConent = UrlEncode(msg, encoding);

                    var url = $"{SmsServiceAddress}?action=send&userid={Setting.SmsAccountCORPID}&account={Setting.SmsAccountName}&password={Setting.SmsAccountPassword}&mobile={number}&content={encodedLimitedConent}";
                    var resultDoc = XDocument.Load(url);
                    var status = resultDoc.Root.Element("returnstatus").Value;
                    var message = resultDoc.Root.Element("message").Value;
                    if (message.ToUpper() == "OK")
                    {
                        smsResult = SmsResult.成功;
                        LogUtility.SmsLogger.Info($"发送短信成功：{content}，号码：{number}。状态：{status}（{message}）。发送通道：JunMei");
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception)
            {
                LogUtility.SmsLogger.Error($"发送短信失败：{content}，号码：{number}。发送通道：JunMei");
            }
            return smsResult;
        }

        public override string GetLastCount()
        {
            var url = $"{SmsServiceAddress}?action=overage&userid={Setting.SmsAccountCORPID}&account={Setting.SmsAccountName}&password={Setting.SmsAccountPassword}";
            var resultDoc = XDocument.Load(url);
            return $"overage: {resultDoc.Root.Element("overage").Value}, sendTotal: {resultDoc.Root.Element("sendTotal").Value}";

        }

        /// <summary>
        /// 接收回复信息
        /// </summary>
        /// <returns></returns>
        public ReplyMessageCollection GetReplyMessages()
        {
            throw new Exception("此方法已修改，请查看文档！");

            //var url =
            //    $"{SmsServiceAddress}/Mo.asp?CORPID={Setting.SmsAccountCORPID}&USERNAME={Setting.SmsAccountName}&PASSWORD={Setting.SmsAccountPassword}";

            //var replyMessageCollection = new ReplyMessageCollection();
            //var wc = new WebClient();
            //var result = wc.DownloadString(url);
            //if (result == "0")
            //{
            //    return replyMessageCollection;
            //}

            //var msgs = result.Split(new[] { "|;|" }, StringSplitOptions.RemoveEmptyEntries);
            //msgs.AsParallel().ForAll(z =>
            //{
            //    var datas = z.Split(new[] { "|^|" }, StringSplitOptions.RemoveEmptyEntries);
            //    var replyMessage = new ReplyMessage()
            //    {
            //        State = int.Parse(datas[0]),
            //        Id = int.Parse(datas[1]),
            //        PhoneNumber = datas[2],
            //        DateTime = DateTime.Parse(datas[3])
            //    };
            //    replyMessageCollection.MsgCollection.Add(replyMessage);
            //});
            //return replyMessageCollection;
        }
    }

}
