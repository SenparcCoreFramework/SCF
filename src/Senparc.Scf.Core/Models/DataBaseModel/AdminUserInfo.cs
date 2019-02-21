using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;
using System;

namespace Senparc.Scf.Core.Models
{
    [Serializable]
    public partial class AdminUserInfo : EntityBase<int>
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }
        public string RealName { get; private set; }
        public string Phone { get; private set; }
        public string Note { get; private set; }
        public DateTime ThisLoginTime { get; private set; }
        public string ThisLoginIp { get; private set; }
        public DateTime LastLoginTime { get; private set; }
        public string LastLoginIp { get; private set; }

        private AdminUserInfo() { }

        public AdminUserInfo(string userName, string password, string realName, string phone, string note)
        {
            UserName = userName?? GenerateUserName();
            PasswordSalt = GeneratePasswordSalt();//生成密码盐
            Password = GetPassword(password, PasswordSalt, true);//生成密码
            RealName = realName;
            Phone = phone;
            Note = note;

            var now = SystemTime.Now.LocalDateTime;
            AddTime = now;
            ThisLoginTime = now;
            LastLoginTime = now;

            //TODO：用户名及密码合规性验证
        }

        private string GetPassword(string password, string salt, bool isMD5Password)
        {
            string md5 = password.ToUpper().Replace("-", "");
            if (!isMD5Password)
            {
                md5 = MD5.GetMD5Code(password, "").Replace("-", ""); //原始MD5
            }
            return MD5.GetMD5Code(md5, salt).Replace("-", ""); //再加密
        }

        /// <summary>
        /// 生成用户名
        /// </summary>
        /// <returns></returns>
        public string GenerateUserName()
        {
            return $"SenparcCoreAdmin{new Random().Next(100).ToString("00")}";
        }

        /// <summary>
        /// 生成密码盐
        /// </summary>
        /// <returns></returns>
        public string GeneratePasswordSalt()
        {
            return DateTime.Now.Ticks.ToString();
        }

        public void UpdateObject(string userName, string password)
        {
            UserName = userName;
            if (!password.IsNullOrEmpty())
            {
                Password = password;
            }
            base.SetUpdateTime();
        }
    }
}
