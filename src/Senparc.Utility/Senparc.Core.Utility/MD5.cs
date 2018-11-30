using System;
using System.Security.Cryptography;
using System.Text;

namespace Senparc.Core.Utility
{
    public static class MD5
    {
        public static string GetMD5Code(string str, string salt)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str + salt)));//MD5加密
            return result;
        }
    }
}
