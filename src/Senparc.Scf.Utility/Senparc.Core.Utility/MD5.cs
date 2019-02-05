using System;
using System.Security.Cryptography;
using System.Text;

namespace Senparc.Scf.Core.Utility
{
    /// <summary>
    /// MD5 加密
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// 获得 SCF 系统内全局一致的加盐的 MD5 加密结果
        /// </summary>
        /// <param name="str"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetMD5Code(string str, string salt)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str + salt)));//MD5加密
            return result;
        }
    }
}
