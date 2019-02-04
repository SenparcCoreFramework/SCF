using Senparc.CO2NET.Extensions;
using System;

namespace Senparc.Scf.Core.Email
{
    /// <summary>
    /// 发送注册激活验证码
    /// </summary>
    public class SendEmailParameter_LiveCode : SendEmailParameter
    {
        public string Password { get; set; }
        public Guid LiveCode { get; set; }
        public string ReturnUrl { get; set; }
        public string EncodedUserName { get; set; }

        public SendEmailParameter_LiveCode(string toEmail, string userName, string password, Guid liveCode, string returnUrl)
        {
            ToEmail = toEmail; UserName = userName;
            Password = password; LiveCode = liveCode; ReturnUrl = returnUrl;
            EncodedUserName = userName.UrlEncode();
        }
    }
    /// <summary>
    /// 通过注册激活验证
    /// </summary>
    public class SendEmailParameter_PassLiveCode : SendEmailParameter
    {
        public string ContinueToDo { get; set; }
        public SendEmailParameter_PassLiveCode(string toEmail, string userName, string continueToDo)
        {
            ToEmail = toEmail; UserName = userName;
            ContinueToDo = continueToDo;
        }
    }
    /// <summary>
    /// 重设密码
    /// </summary>
    public class SendEmailParameter_ResetPassword : SendEmailParameter
    {
        public string Code { get; set; }

        public SendEmailParameter_ResetPassword(string toEmail, string userName, string code)
        {
            ToEmail = toEmail; UserName = userName; Code = code;
        }
    }


    /// <summary>
    /// 邀请码
    /// </summary>
    public class SendEmailParameter_InviteCode : SendEmailParameter
    {
        public string Code { get; set; }
        public string RegUrl { get; set; }
        public SendEmailParameter_InviteCode(string toEmail, string userName, string code, string regUrl)
        {
            ToEmail = toEmail; UserName = userName; Code = code; RegUrl = regUrl;
        }
    }

    /// <summary>
    /// 订单创建
    /// </summary>
    public class SendEmailParameter_OrderCreate : SendEmailParameter
    {
        public string OrderNumber { get; set; }
        public string Money { get; set; }
        public string Note { get; set; }
        public SendEmailParameter_OrderCreate(string toEmail, string userName, string orderNumber, decimal money, string note)
        {
            ToEmail = toEmail; UserName = userName;
            OrderNumber = orderNumber;
            Money = money.ToString("C");
            Note = note;
        }
    }

    /// <summary>
    /// 订单创建支付成功
    /// </summary>
    public class SendEmailParameter_OrderPaySuccess : SendEmailParameter
    {
        public string OrderNumber { get; set; }
        public string Money { get; set; }
        public string Note { get; set; }
        public SendEmailParameter_OrderPaySuccess(string toEmail, string userName, string orderNumber, decimal money, string note)
        {
            ToEmail = toEmail; UserName = userName;
            OrderNumber = orderNumber;
            Money = money.ToString("C");
            Note = note;
        }
    }

    /// <summary>
    /// 订单取消
    /// </summary>
    public class SendEmailParameter_OrderCancelled : SendEmailParameter
    {
        public string OrderNumber { get; set; }
        public string Money { get; set; }
        public string Note { get; set; }
        public SendEmailParameter_OrderCancelled(string toEmail, string userName, string orderNumber, decimal money, string note)
        {
            ToEmail = toEmail; UserName = userName;
            OrderNumber = orderNumber;
            Money = money.ToString("C");
            Note = note;
        }
    }

}
