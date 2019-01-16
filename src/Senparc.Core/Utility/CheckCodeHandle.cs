using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Senparc.CO2NET.Cache;
using Senparc.Core.Cache.BaseCache;
using System;
using System.Collections.Generic;

namespace Senparc.Core.Utility
{
    /// <summary>
    /// 验证码用途（分类）
    /// </summary>
    public enum CheckCodeKind
    {
        Reg = 0,
        Login = 1,
        SMS = 2
    }

    public class CheckCodeHandle
    {
        private CheckCodeKind _checkCodeKind;
        private const string COOKIE_NAME_PREFIX = "senparccheckcodeverify_";
        private string _cookieName;
        private HttpContext _httpContext;
        private IMemoryCache _cache;
        private string _checkCodeSalt = "_senparccheckcode";
        private Dictionary<string, string> checkedCodeCollection
        {
            get
            {
                var cacheStrategy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
                if (cacheStrategy.Get("SenparcCheckCodeSuccessCollection") == null)
                {
                    cacheStrategy.Set("SenparcCheckCodeSuccessCollection", new Dictionary<string, string>(), TimeSpan.FromMinutes(20));
                }
                return _cache.Get<Dictionary<string, string>>("SenparcCheckCodeSuccessCollection");
            }
        }

        public CheckCodeHandle(CheckCodeKind checkCodeKind, HttpContext httpContext)
        {
            _checkCodeKind = checkCodeKind;
            _cookieName = COOKIE_NAME_PREFIX + (int)checkCodeKind;
            _httpContext = httpContext;

        }

        /// <summary>
        /// 获取验证码并记录cookie
        /// </summary>
        /// <param name="checkCodeKind"></param>
        /// <returns></returns>
        public string GenerateCheckCode()
        {
            //int number;
            char code;
            string checkcode = string.Empty;
            System.Random random = new Random();

            //string[] str = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string str1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";//abcdefghijklmnopqrstuvwxyz

            for (int i = 0; i < 4; i++)//原始值：5
            {

                //code = (char)str[random.Next(0, 61)];
                code = str1[random.Next(0, str1.Length - 1)];

                checkcode += code.ToString();

                //number = random.Next();
                //if (number % 2 == 0
                //    code = (char)('0' + (char)(number % 10));
                //else
                //    code = (char)('0' + (char)(number % 10));
                //checkcode += code.ToString();
            }

            this.SaveToCookie(checkcode);

            //base.Response.Write("<script>alert('sdfdssf');</script>" + checkcode.ToString());
            //Response.Cookies.Add(new HttpCookie("checkcode", checkcode));
            return checkcode;
        }

        public void SaveToCookie(string checkCode)
        {
            DateTimeOffset to = new DateTimeOffset(DateTime.Now.AddDays(1));
            var verifyCode = GenerateVerifyCode(checkCode, null);
            var value = $"checkcode={verifyCode}";
            _httpContext.Response.Cookies.Append(this._cookieName, value, new CookieOptions() { Expires = to });//确定写入cookie中
        }

        public bool ValidateCheckCode(string inputCheckCode)
        {
            if (string.IsNullOrEmpty(inputCheckCode))
            {
                return false;
            }

            //验证码验证
            var myCookie = _httpContext.Request.Cookies[this._cookieName];

            if (myCookie == null || !myCookie.StartsWith("checkcode="))
            {
                return false;
            }

            string checkCodeVerify = myCookie.Split('=')[1];
            if (string.IsNullOrEmpty(checkCodeVerify) || checkCodeVerify.Length < 32)
            {
                return false;
            }
            if (checkedCodeCollection.ContainsKey(checkCodeVerify))
            {
                return false;//如果已经参与过验证，则不能再次使用
            }
            var verifyCode = GenerateVerifyCode(inputCheckCode.Trim(), checkCodeVerify.Substring(checkCodeVerify.Length - 32, 32));
            if (verifyCode.ToUpper() != checkCodeVerify.ToUpper())
            {
                return false;
            }
            checkedCodeCollection.Add(checkCodeVerify, inputCheckCode);
            return true;

            //_httpContext.Response.Write("<script language = 'javascript'> alert('您的浏览器不支持coolie，请联系客服！');</script>");
        }

        public string GenerateVerifyCode(string inputCheckCode, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString().Replace("-", "");
            }
            string verifyFormat = "{0}{1}{2}";
            string checkcodeEncoding = MD5.GetMD5Code(inputCheckCode.ToUpper(), key.ToUpper() + _checkCodeSalt).Replace("-", "");
            string verifyCode = string.Format(verifyFormat,
                                    ((int)_checkCodeKind).ToString(),
                                    checkcodeEncoding,
                                    key//密匙
                                    );
            return verifyCode;
        }
    }
}
