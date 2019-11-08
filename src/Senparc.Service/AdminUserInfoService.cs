using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Senparc.Core.Models;
using Senparc.Repository;
using Senparc.Scf.Core.Config;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;
using Senparc.Scf.Log;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Debug;

namespace Senparc.Service
{
    public class AdminUserInfoService : ClientServiceBase<AdminUserInfo>
    {

        private readonly Lazy<IHttpContextAccessor> _contextAccessor;
        public AdminUserInfoService(AdminUserInfoRepository repository, Lazy<IHttpContextAccessor> httpContextAccessor, IServiceProvider serviceProvider)
            : base(repository, serviceProvider)
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

        public async Task<AdminUserInfo> GetUserInfo(string userName)
        {
            var obj = GetObject(z => z.UserName.Equals(userName.Trim()));
            return obj;
        }

        public AdminUserInfo GetUserInfo(string userName, string password)
        {
            AdminUserInfo userInfo = GetObject(z => z.UserName.Equals(userName), null);
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

        public async Task Logout()
        {
            try
            {
                await _contextAccessor.Value.HttpContext.SignOutAsync(SiteConfig.ScfAdminAuthorizeScheme);
            }
            catch (Exception ex)
            {
                LogUtility.AdminUserInfo.ErrorFormat("退出登录失败。", ex);
            }
        }

        public virtual void Login(AdminUserInfo userInfo, bool rememberMe)
        {
            #region 使用 .net core 的方法写入 cookie 验证信息

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.UserName),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString(), ClaimValueTypes.Integer),
                new Claim("AdminMember", "", ClaimValueTypes.String)
            };
            var identity = new ClaimsIdentity(SiteConfig.ScfAdminAuthorizeScheme);
            identity.AddClaims(claims);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                IsPersistent = false,
            };

            Logout(); //退出登录
            _contextAccessor.Value.HttpContext.SignInAsync(SiteConfig.ScfAdminAuthorizeScheme, new ClaimsPrincipal(identity), authProperties);

            #endregion
        }

        public bool CheckPassword(string userName, string password)
        {
            var userInfo = GetObject(z => z.UserName.Equals(userName));
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
                Login(userInfo, rememberMe);
                return userInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="objDto"></param>
        public void CreateAdminUserInfo(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            string userName = objDto.UserName;
            string password = objDto.Password;
            var obj = new AdminUserInfo(ref userName, ref password, null, null, objDto.Note);
            SaveObject(obj);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="objDto"></param>
        public void UpdateAdminUserInfo(CreateOrUpdate_AdminUserInfoDto objDto)
        {
            var obj = this.GetObject(z => z.Id == objDto.Id);
            if (obj == null)
            {
                throw new Exception("用户信息不存在！");
            }
            obj.UpdateObject(objDto);
            SaveObject(obj);
        }

        public CreateOrUpdate_AdminUserInfoDto GetAdminUserInfo(int id,params string[] includes)
        {
            var obj = GetObject(z => z.Id == id, includes: includes);

            var objDto = base.Mapper.Map<CreateOrUpdate_AdminUserInfoDto>(obj);

            return objDto;
        }

        public List<AdminUserInfo> GetAdminUserInfo(List<int> ids,params string[] includes)
        {
            return GetFullList(z => ids.Contains(z.Id), z => z.Id, Scf.Core.Enums.OrderingType.Ascending, includes: includes);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public AdminUserInfo Init(out string userName, out string password)
        {
            userName = null;
            password = null;

            var oldAdminUserInfo = GetObject(z => true);
            if (oldAdminUserInfo != null)
            {
                return null;
            }
            var adminUserInfo = new AdminUserInfo(ref userName, ref password, null, null, "初始化数据");
            SaveObject(adminUserInfo);
            return adminUserInfo;
        }

        public void DeleteObject(int id)
        {
            var obj = GetObject(z => z.Id == id);
            if (obj == null)
            {
                throw new Exception("用户信息不存在！");
            }
            DeleteObject(obj);
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
