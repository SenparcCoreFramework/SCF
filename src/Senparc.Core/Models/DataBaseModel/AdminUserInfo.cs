using Senparc.Scf.Core.Models;
using System;

namespace Senparc.Core.Models
{
    [Serializable]
    public partial class AdminUserInfo: EntityBase<int>
    {
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
    }
}
