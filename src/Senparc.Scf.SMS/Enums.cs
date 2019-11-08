using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.SMS
{

    /// <summary>
    /// 短信平台类型
    /// </summary>
    public enum SmsPlatformType
    {
        Unknow = -1,
        JunMei = 0,
        Fissoft = 1,
        SMS1 = 10,
        SMS2 = 20,
        SMS3 = 30,
        SMS4 = 40
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
}
