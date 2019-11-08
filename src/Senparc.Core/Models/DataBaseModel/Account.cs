using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;

namespace Senparc.Core.Models
{
    [Serializable]
    public partial class Account : EntityBase<int>
    {
        public Account()
        {
            PointsLogs = new List<PointsLog>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public bool? PhoneChecked { get; set; }

        public string Email { get; set; }
        public bool? EmailChecked { get; set; }
        public string PicUrl { get; set; }
        public string HeadImgUrl { get; set; }
        public decimal Package { get; set; }
        public decimal Balance { get; set; }
        public decimal LockMoney { get; set; }
        public byte Sex { get; set; }
        public string QQ { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public DateTime ThisLoginTime { get; set; }
        public string ThisLoginIp { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public decimal Points { get; set; }
        public DateTime? LastWeixinSignInTime { get; set; }
        public int WeixinSignTimes { get; set; }
        public string WeixinUnionId { get; set; }
      
        public string WeixinOpenId { get; set; }
        
        public ICollection<PointsLog> PointsLogs { get; set; }
        public ICollection<AccountPayLog> AccountPayLogs { get; set; }
    }
}
