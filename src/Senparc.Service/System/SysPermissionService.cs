using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Core.WorkContext.Provider;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Senparc.Service
{
    public class SysPermissionService : ClientServiceBase<SysPermission>
    {
        private readonly IDistributedCache _distributedCache;

        public SysMenuService _sysMenuService { get; }

        private readonly IAdminWorkContextProvider _adminWorkContextProvider;
        private readonly SysRoleService _sysRoleService;
        private const string PermissionKey = "Permission";

        public SysPermissionService(ClientRepositoryBase<SysPermission> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            this._distributedCache = _serviceProvider.GetService<IDistributedCache>();
            this._sysMenuService = _serviceProvider.GetService<SysMenuService>();
            this._adminWorkContextProvider = _serviceProvider.GetService<IAdminWorkContextProvider>();
            this._sysRoleService = _serviceProvider.GetService<SysRoleService>();
        }

        public async Task<bool> HasPermissionAsync(string url)
        {
            IEnumerable<string> roleIds = _adminWorkContextProvider.GetAdminWorkContext().RoleCodes;
            string str = await _distributedCache.GetStringAsync(PermissionKey);
            IEnumerable<SysPermissionDto> permissions;
            if (string.IsNullOrEmpty(str))
            {
                permissions = await DbToCacheAsync();
            }
            else
            {
                permissions = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SysPermissionDto>>(str);
            }
            permissions.Where(_ => roleIds.Contains(_.RoleCode));
            if (!permissions.Any())
            {
                return false;
            }
            IEnumerable<SysMenuDto> sysMenus = await _sysMenuService.GetMenuDtoByCacheAsync();
            var urls = from menu in sysMenus
                       join permission in permissions on menu.Id equals permission.PermissionId
                       where !string.IsNullOrEmpty(menu.Url)
                       select menu.Url.ToLower();

            if (!urls.Any() || !urls.Contains(url.ToLower()))
            {
                return false;
            }
            return true;
        }

        //public async Task<IEnumerable<SysMenuDto>> GetUserMenuDtosAsync()
        //{
        //    IEnumerable<string> roleIds = _adminWorkContextProvider.GetAdminWorkContext().RoleCodes;
        //    string str = await _distributedCache.GetStringAsync(PermissionKey);
        //    IEnumerable<SysPermissionDto> permissions = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SysPermissionDto>>(str).Where(_ => roleIds.Contains(_.RoleCode));
        //    IEnumerable<SysMenuDto> sysMenus = await _sysMenuService.GetMenuDtoByCacheAsync();
        //    var urls = from menu in sysMenus
        //               join permission in permissions on menu.Id equals permission.PermissionId
        //               where !string.IsNullOrEmpty(menu.Url)
        //               select menu;

        //    return urls;
        //}

        public async Task<IEnumerable<SysPermissionDto>> DbToCacheAsync()
        {
            IEnumerable<SysPermission> permissions = await GetFullListAsync(_ => true);
            IEnumerable<SysPermissionDto> permissionDtos = Mapper.Map<IEnumerable<SysPermissionDto>>(permissions);
            await _distributedCache.RemoveAsync(PermissionKey);
            await _distributedCache.SetStringAsync(PermissionKey, Newtonsoft.Json.JsonConvert.SerializeObject(permissionDtos));
            return permissionDtos;
        }

        public async Task InitPermissionCache()
        {
            await DbToCacheAsync();
            await _sysMenuService.GetMenuDtoByCacheAsync(true);
        }

        /// <summary>
        /// 添加权限信息
        /// </summary>
        /// <param name="sysMenuDto"></param>
        /// <returns></returns>
        public async Task AddAsync(IEnumerable<SysPermissionDto> sysMenuDto)
        {
            List<SysPermission> sysRoleMenus = new List<SysPermission>();
            string RoleId = sysMenuDto.FirstOrDefault().RoleId;
            SysRole sysRole = await _sysRoleService.GetObjectAsync(_ => _.Id == RoleId);
            //
            foreach (var item in sysMenuDto)
            {
                item.RoleCode = sysRole.RoleCode;
                SysPermission sysPermission = new SysPermission(item);
                sysRoleMenus.Add(sysPermission);
            }

            await BeginTransactionAsync(async () => 
            {
                IEnumerable<SysPermission> entitis = await GetFullListAsync(_ => _.RoleId == sysMenuDto.FirstOrDefault().RoleId);
                await DeleteAllAsync(entitis);
                await SaveObjectListAsync(sysRoleMenus);
                await DbToCacheAsync();//暂时
            });
        }

        /// <summary>
        /// 获取当前用户的权限
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysPermissionDto>> GetUserSysPermissionDtosAsync()
        {
            IEnumerable<string> roleIds = _adminWorkContextProvider.GetAdminWorkContext().RoleCodes;
            string str = await _distributedCache.GetStringAsync(PermissionKey);
            IEnumerable<SysPermissionDto> permissions;
            if (string.IsNullOrEmpty(str))
            {
                permissions = await DbToCacheAsync();
            }
            else
            {
                permissions = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SysPermissionDto>>(str);
            }
            return permissions.Where(_ => roleIds.Contains(_.RoleCode));
        }

        public async Task<bool> HasPermissionByButtonCodeAsync(string code, string url)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            IEnumerable<string> roleIds = _adminWorkContextProvider.GetAdminWorkContext().RoleCodes;
            string str = await _distributedCache.GetStringAsync(PermissionKey);
            IEnumerable<SysPermissionDto> permissions;
            if (string.IsNullOrEmpty(str))
            {
                permissions = await DbToCacheAsync();
            }
            else
            {
                permissions = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SysPermissionDto>>(str);
            }
            permissions.Where(_ => roleIds.Contains(_.RoleCode));
            if (!permissions.Any())
            {
                return false;
            }
            IEnumerable<SysMenuDto> sysMenus = await _sysMenuService.GetMenuDtoByCacheAsync();
            var resourceCodes = from menu in sysMenus
                       join permission in permissions on menu.Id equals permission.PermissionId
                       where !string.IsNullOrEmpty(menu.ResourceCode) && !permission.IsMenu
                       select menu.ResourceCode;

            IEnumerable<SysMenuDto> sysMenuDtos = await _sysMenuService.GetMenuDtoByCacheAsync();
            SysMenuDto sysMenuDto = sysMenuDtos.FirstOrDefault(_ => _.Url?.ToLower() == url.ToLower() && _.IsMenu);
            if (sysMenuDto == null)
            {
                return false;
            }
            bool isInUrl = sysMenuDtos.Any(_ => _.ParentId == sysMenuDto.Id && !_.IsMenu);
            bool has = resourceCodes.Any(_ => _ == code);
            return has && isInUrl;
        }

        /// <summary>
        /// 获取当前用户可以看见的菜单（可见）
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysMenuTreeItemDto>> GetMenuTreeDtoByCacheAsync()
        {
            //IEnumerable<SysMenuTreeItemDto> sysMenuTreeItems = null;//
            IEnumerable<SysMenuDto> sysMenuDtos = await _sysMenuService.GetMenuDtoByCacheAsync();
            IEnumerable<SysPermissionDto> permissions = await GetUserSysPermissionDtosAsync();
            var urls = from menu in sysMenuDtos
                       join permission in permissions on menu.Id equals permission.PermissionId
                       where menu.Visible
                       select menu;
            return _sysMenuService.GetSysMenuTreesMainRecursive(urls);
        }
    }
}
