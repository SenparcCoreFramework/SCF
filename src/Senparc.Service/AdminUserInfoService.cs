using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Core.Utility;
using Senparc.Log;
using Senparc.Repository;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Senparc.Service
{
    public class AdminUserInfoService : BaseClientService<AdminUserInfo>
    {

        private readonly Lazy<IHttpContextAccessor> _contextAccessor;
        public AdminUserInfoService(AdminUserInfoRepository repository, Lazy<IHttpContextAccessor> httpContextAccessor) : base(repository)
        {
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckUserNameExisted(long id, string userName)
        {
            return GetObject(z => z.Id != id && z.UserName.Equals(userName.Trim(), StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        public AdminUserInfo GetUserInfo(string userName)
        {
            return GetObject(z => z.UserName.Equals(userName.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public AdminUserInfo GetUserInfo(string userName, string password)
        {
            AdminUserInfo userInfo = GetObject(z => z.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase), null);
            if (userInfo == null)
            {
                return null;
            }
            var codedPassword = GetPassword(password, userInfo.PasswordSalt, false);
            return userInfo.Password == codedPassword ? userInfo : null;
        }

        public string GetPassword(string password, string salt, bool isMD5Password)
        {
            string md5 = password.ToUpper().Replace("-", "");
            if (!isMD5Password)
            {
                md5 = MD5.GetMD5Code(password, "").Replace("-", ""); //原始MD5
            }
            return MD5.GetMD5Code(md5, salt).Replace("-", ""); //再加密
        }

        public void Logout()
        {
            try
            {
                _contextAccessor.Value.HttpContext.SignOutAsync(AdminAuthorizeAttribute.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                LogUtility.AdminUserInfo.ErrorFormat("退出登录失败。", ex);
            }
        }

        public virtual void Login(string userName, bool rememberMe)
        {
            #region 使用 .net core 的方法写入 cookie 验证信息

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("AdminMember", "", ClaimValueTypes.String)
            };
            var identity = new ClaimsIdentity(AdminAuthorizeAttribute.AuthenticationScheme);
            identity.AddClaims(claims);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                IsPersistent = false,
            };

            Logout(); //退出登录
            _contextAccessor.Value.HttpContext.SignInAsync(AdminAuthorizeAttribute.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

            #endregion
        }

        public bool CheckPassword(string userName, string password)
        {
            var userInfo = GetObject(z => z.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (userInfo == null)
            {
                return false;
            }
            var codedPassword = GetPassword(password, userInfo.PasswordSalt, false);
            return userInfo.Password == codedPassword;
        }

        public AdminUserInfo TryLogin(string userNameOrEmail, string password, bool rememberMe)
        {
            AdminUserInfo userInfo = GetUserInfo(userNameOrEmail, password);
            if (userInfo != null)
            {
                Login(userInfo.UserName, rememberMe);
                return userInfo;
            }
            else
            {
                return null;
            }
        }

        public AdminUserInfo GetAdminUserInfo(int id, string[] includes = null)
        {
            return GetObject(z => z.Id == id, includes: includes);
        }

        public List<AdminUserInfo> GetAdminUserInfo(List<int> ids, string[] includes = null)
        {
            return GetFullList(z => ids.Contains(z.Id), z => z.Id, Core.Enums.OrderingType.Ascending, includes: includes);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public AdminUserInfo Init(out string userName, out string password)
        {
            var oldAdminUserInfo = GetObject(z => true);
            if (oldAdminUserInfo != null)
            {
                userName = null;
                password = null;
                return null;
            }

            var salt = DateTime.Now.Ticks.ToString();
            userName = $"SenparcCoreAdmin{new Random().Next(100).ToString("00")}";
            password = Guid.NewGuid().ToString("n").Substring(0, 8);

            var adminUserInfo = new AdminUserInfo()
            {
                UserName = userName,
                Password = GetPassword(password, salt, false),
                PasswordSalt = salt,
                Note = "初始化数据",
                AddTime = DateTime.Now,
                ThisLoginTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
            };

            SaveObject(adminUserInfo);
            return adminUserInfo;
        }

        public override void SaveObject(AdminUserInfo obj)
        {
            var isInsert = base.IsInsert(obj);
            base.SaveObject(obj);
            LogUtility.WebLogger.InfoFormat("AdminUserInfo{2}：{0}（ID：{1}）", obj.UserName, obj.Id, isInsert ? "新增" : "编辑");
        }

        public override void DeleteObject(AdminUserInfo obj)
        {
            base.DeleteObject(obj);
            LogUtility.WebLogger.InfoFormat("AdminUserInfo被删除：{0}（ID：{1}）", obj.UserName, obj.Id);
        }
    }
}
