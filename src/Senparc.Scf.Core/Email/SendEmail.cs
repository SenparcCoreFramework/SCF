using System;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Reflection;

namespace Senparc.Scf.Core.Email
{
    using Senparc.Scf.Core.Models;
    using Senparc.Scf.Core.Config;
    using Senparc.Scf.Core.Extensions;
    using Senparc.Scf.Core.Enums;
    using Senparc.Scf.Core.Cache;
    using Senparc.Scf.Core.Utility;
    using Senparc.Scf.Log;
    using System.IO;
    using Senparc.CO2NET.Extensions;

    interface ISendEmail
    {
        bool DoSendEmail(string to, string subject, string body, string username, bool useEmailTemplate, EmailAccountType emailUserType, bool fromCache);
        bool Send<T>(T sendEmailParameter, bool lineInCache) where T : SendEmailParameter;
        string ReplaceHolderValue<T>(T sendEmailParameter, string content) where T : SendEmailParameter;
        string GetFilledSubject<T>(T sendEmailParameter) where T : SendEmailParameter;
        string GetFilledBody<T>(T sendEmailParameter) where T : SendEmailParameter;
    }

    public class SendEmail : ISendEmail
    {
        private XmlConfig_Email ConfigNode { get; set; }
        private string Subject { get; set; }
        private string Body { get; set; }

        /// <summary>
        /// 已经发送的标题
        /// </summary>
        public string SentSubject { get; set; }
        /// <summary>
        /// 已经发送的内容
        /// </summary>
        public string SentBody { get; set; }


        public bool UseEmailTemplate { get; set; }

        public SendEmail(SendEmailType? toUse)
        {
            if (toUse != null)
            {
                ConfigNode = XmlConfig.GetEmailConfig((SendEmailType)toUse);//获取模板的节点内容
                Subject = ConfigNode.Subject;
                Body = ConfigNode.Body.HtmlDecode();
            }
            UseEmailTemplate = true;
        }

        /// <summary>
        /// 获取Email账户信息
        /// </summary>
        /// <returns></returns>
        private EmailUser GetEmailUser(EmailAccountType emailUserType)
        {
            //emailUserType = emailUserType == EmailUserType.Default ? EmailUserType._163 : emailUserType;

            XmlDataContext xmlCtx = new XmlDataContext();
            var emailUserList = xmlCtx.GetXmlList<EmailUser>();
            var emailUser = emailUserList.FirstOrDefault(z => z.Name == emailUserType.ToString());
            if (emailUser == null)
            {
                LogUtility.EmailLogger.ErrorFormat("EmailUserType类型错误。传入类型：{0}", emailUserType.ToString());
            }
            return emailUser;
        }

        #region ISendEmail 成员

        public bool DoSendEmail(string to, string subject, string body, string username, bool useEmailTemplate, EmailAccountType emailUserType, bool fromCache)
        {
            bool sendSuccess = false;
            string smtpEmailAddress = null;
            //if (emailUserType == EmailUserType.Default && to.ToUpper().EndsWith("@QQ.COM"))
            //{
            //    //useBackupEmail = !useBackupEmail;//如果是qq邮箱，则切换
            //    emailUserType = EmailUserType._163;
            //}

            //if (emailUserType == EmailUserType.Default)
            //{
            //    emailUserType = EmailUserType.KJH8;//默认使用KJH8发送
            //}

            try
            {
                body = this.GetEmailTemplate(body, useEmailTemplate);

                EmailUser targetEmailUser = this.GetEmailUser(emailUserType);
                smtpEmailAddress = targetEmailUser.EmailAddress;

                MailMessage m_message = new MailMessage();
                m_message.From = new MailAddress(targetEmailUser.EmailAddress, targetEmailUser.DisplayName);
                m_message.To.Add(new MailAddress(to));
                m_message.Subject = subject;
                m_message.IsBodyHtml = true;//允许使用html格式
                m_message.Body = body;

                SmtpClient m_smtpClient = new SmtpClient();
                m_smtpClient.Host = targetEmailUser.SmtpHost;
                m_smtpClient.Port = targetEmailUser.SmtpPort;
                if (targetEmailUser.NeedCredentials)
                {
                    m_smtpClient.Credentials = new System.Net.NetworkCredential(targetEmailUser.EmailAddress, targetEmailUser.Password);
                }

                m_smtpClient.Send(m_message);

                sendSuccess = true;
            }
            catch (Exception e)
            {
                LogUtility.EmailLogger.ErrorFormat($"TO:{to},Subject:{subject},Username:{username},Message:{e.Message}", e);
                sendSuccess = false;
            }
            finally
            {
                string cacheUsed = fromCache ? "缓存" : "直接发送";
                if (sendSuccess)
                {
                    //记录到AutoSendEmail中进行备份
                    XmlDataContext xmlCtx = new XmlDataContext();
                    AutoSendEmailBak emailBak = new AutoSendEmailBak()
                    {
                        Address = to,
                        Body = body,
                        Subject = subject,
                        SendTime = DateTime.Now,
                        UserName = username
                    };
                    xmlCtx.Insert(emailBak);

                    LogUtility.EmailLogger.Info($"发送Email成功({cacheUsed})。To:{to}，Subject:{subject}，UserName:{username}。by:{smtpEmailAddress}");
                }
                else
                {
                    LogUtility.EmailLogger.Error($"发送Email失败({cacheUsed})。To:{to}，Subject:{subject}，UserName:{username}。by:{smtpEmailAddress}");
                }
            }
            return sendSuccess;
        }

        /// <summary>
        /// 发送Email，只要条件符合，立即发送（当处于缓存中时，只要轮询到，也立即尝试发送）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendEmailParameter"></param>
        /// <param name="lineInCache"></param>
        /// <returns></returns>
        public bool Send<T>(T sendEmailParameter, bool lineInCache) where T : SendEmailParameter
        {
            return this.Send(sendEmailParameter, lineInCache, true, EmailAccountType.Default);
        }

        ///// <summary>
        ///// 发送Email，只要条件符合，立即发送（当处于缓存中时，只要轮询到，也立即尝试发送）
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="sendEmailParameter"></param>
        ///// <param name="lineInCache"></param>
        ///// <param name="useBackupEmail">使用备用Email发送（如果lineInCache=True，则由缓存处理模块决定）</param>
        ///// <returns></returns>
        //public bool Send<T>(T sendEmailParameter, bool lineInCache, bool useBackupEmail) where T : SendEmailParameter
        //{
        //    return this.Send(sendEmailParameter, lineInCache, true, useBackupEmail);
        //}

        /// <summary>
        /// 发送Email
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendEmailParameter"></param>
        /// <param name="lineInCache"></param>
        /// <param name="sendImmediately">是否立即发送（仅当lineInCache=true时有效）</param>
        /// <param name="useBackupEmail">使用备用Email发送</param>
        /// <returns></returns>
        public bool Send<T>(T sendEmailParameter, bool lineInCache, bool sendImmediately, EmailAccountType emailUserType) where T : SendEmailParameter
        {
            SentSubject = null;
            SentBody = null;
            try
            {
                if (sendEmailParameter.ToEmail.IsNullOrEmpty() /*|| !sendEmailParameter.ToEmail.ValidateEmailFormat()*/)
                {
                    throw new Exception("ToEmail参数为空");
                }

                //替换
                SentSubject = ReplaceHolderValue(sendEmailParameter, Subject);
                SentBody = ReplaceHolderValue(sendEmailParameter, Body);

                bool sendImmediatelySuccess = true;
                if (!lineInCache)
                {
                    //立即发送，发送失败的邮件会列入缓存发送
                    sendImmediatelySuccess = DoSendEmail(sendEmailParameter.ToEmail, SentSubject, SentBody, sendEmailParameter.UserName, true, emailUserType, lineInCache);
                }

                //直接放入缓存，或将发送失败的邮件内容放入缓存
                if (lineInCache || !sendImmediatelySuccess)
                {
                    //存入缓存
                    SendEmailCache cache = new SendEmailCache();
                    AutoSendEmail email = new AutoSendEmail
                    {
                        UserName = sendEmailParameter.UserName,
                        Address = sendEmailParameter.ToEmail,
                        Subject = SentSubject,
                        Body = this.GetEmailTemplate(SentBody, this.UseEmailTemplate),//内容 + 模板
                        SendCount = sendImmediately ? 0 : (SiteConfig.MaxSendEmailTimes + 200),//如果不要求马上发送，则仅存于缓存中
                        LastSendTime = DateTime.Now
                    };
                    bool insertSuccess = cache.InsertEmail(email);
                    if (!insertSuccess)
                    {
                        LogUtility.EmailLogger.Error($"Email加入缓存失败！Email:{sendEmailParameter.ToEmail},Subject:{SentSubject}\r\n内容：\r\n{SentBody}");
                    }
                    return insertSuccess;
                }
                else
                {
                    return sendImmediatelySuccess;
                }
            }
            catch (Exception e)
            {
                LogUtility.EmailLogger.ErrorFormat(
                    $"发送Email出错：{e.Message}，Email:{sendEmailParameter.ToEmail},Username:{sendEmailParameter.UserName}，" +
                    $"Subject:{SentSubject}\r\n内容：\r\n{SentBody}", e);
                return false;
            }
        }

        public string ReplaceHolderValue<T>(T sendEmailParameter, string content) where T : SendEmailParameter
        {
            foreach (var member in sendEmailParameter.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
            {
                object value = member.GetValue(sendEmailParameter, null);
                string valueString = (value ?? "").ToString();
                //switch (value.GetType().Name)
                //{
                //    case "Double":
                //        valueString = ((double)value).ToString("###,###.###");
                //        break;
                //    case "Int16":
                //    case "Int32":
                //    case "Int64":
                //        valueString = string.Format("{0:N0}", value);//如果碰到ID，不能使用###,###的格式
                //        break;
                //    default:
                //        valueString = value.ToString();
                //        break;
                //}
                content = content.Replace("{$" + member.Name.ToLower() + "$}", valueString);
            }
            content = content.Replace("{$now$}", DateTime.Now.ToString());
            return content;
        }

        /// <summary>
        /// 获取已经填充数据的标题
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendEmailParameter"></param>
        /// <returns></returns>
        public string GetFilledSubject<T>(T sendEmailParameter) where T : SendEmailParameter
        {
            return ReplaceHolderValue(sendEmailParameter, this.Subject);
        }
        /// <summary>
        /// 获取已经填充数据的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendEmailParameter"></param>
        /// <returns></returns>
        public string GetFilledBody<T>(T sendEmailParameter) where T : SendEmailParameter
        {
            return ReplaceHolderValue(sendEmailParameter, this.Body);
        }


        #endregion


        #region Email模板相关

        private string GetEmailTemplate(string body, bool usingEmailTemplate)
        {
            if (usingEmailTemplate)
            {
                //读取模板
                using (var srEmailTemplate = new StreamReader(CO2NET.Utilities.ServerUtility.ContentRootMapPath("~/App_Data/Template/EmailTemplete.htm"), Encoding.UTF8))
                {
                    string emailTemplate = srEmailTemplate.ReadToEnd();
                    this.InsertTemplateValue(ref emailTemplate, "body", body);
                    this.InsertTemplateValue(ref emailTemplate, "longdate", DateTime.Now.ToLongDateString());
                    this.InsertTemplateValue(ref emailTemplate, "year", DateTime.Now.Year.ToString());
                    body = emailTemplate;
                }
            }
            return body;
        }

        private void InsertTemplateValue(ref string template, string valueName, string value)
        {
            if (template.IsNullOrEmpty())
            {
                throw new Exception("Email模板未载入！");
            }
            template = template.Replace("{$" + valueName.ToLower() + "$}", value);
        }
        #endregion


    }


    public class SendEmailFactory
    {

    }
}
