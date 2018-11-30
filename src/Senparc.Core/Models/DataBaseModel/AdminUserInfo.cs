using System;

namespace Senparc.Core.Models
{
    public partial class AdminUserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public DateTime ThisLoginTime { get; set; }
        public string ThisLoginIp { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime AddTime { get; set; }
    }
}
