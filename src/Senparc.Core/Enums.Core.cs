namespace Senparc.Core.Enums
{
    public static class Enums
    {
        public static readonly string[] DayOfWeekString = new[]
        {
            "星期日",
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六"
        };
    }

    public enum MessageType
    {
        danger,
        warning,
        info,
        success
    }

    /// <summary>
    /// 短信平台类型
    /// </summary>
    public enum SmsPlatformType
    {
        Unknow = -1,
        JunMei = 0,
        Fissoft = 1
    }

    /// <summary>
    /// Email账户
    /// </summary>
    public enum EmailAccountType
    {
        Default,
        _163,
        Souidea
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum OrderingType
    {
        Ascending = 0,
        Descending = 1
    }

    /// <summary>
    /// Email设置类型
    /// </summary>
    public enum SendEmailType
    {
        Test = 0,
        CustomEmail,
        LiveCode,
        PassLiveCode,
        ResetPassword,
        InviteCode,
        OrderCreate,
        OrderPaySuccess,
        OrderCancelled,
        WeixinStat, //微信统计
        AppStatusChanged, //应用状态改变
    }

    /// <summary>
    /// 短信发送状态
    /// </summary>
    public enum SmsResult
    {
        未知错误 = 0,
        成功 = 1,
        访问数据库写入数据错误 = -1,
        一次发送的手机号码过多 = -3,
        内容包含不合法文字 = -4,
        登录账户错误 = -5,
        手机号码不合法黑名单 = -9,
        号码太长不能超过100条一次提交 = -10,
        内容太长 = -11,
        余额不足 = -13,
        子号号不正确 = -14,
        参数不全 = -999,
        超过当天请求限制 = -998,
        请在60秒后重试 = -997
    }

    public enum Account_RegisterWay
    {
        官网注册 = 0,
        快捷注册 = 1,
        QQ注册 = 2,
        微博注册 = 3,
        微信自动注册 = 4
    }


    /// <summary>
    /// Meta类型
    /// </summary>
    public enum MetaType
    {
        keywords,
        description
    }
    public enum AccountPayLog_PayType
    {
        网银支付 = 0,
        支付宝 = 1,
        微信支付 = 2
    }
    public enum AccountPayLog_Status
    {
        未支付 = 0,
        已支付 = 1,
        已取消 = 2,
        已冻结 = 3
    }

    #region 实体属性



    #endregion
}