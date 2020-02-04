using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Senparc.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SysMenuService : ClientServiceBase<SysMenu>
    {
        private readonly SysButtonService _sysButtonService;
        private readonly IDistributedCache _distributedCache;

        public const string MenuCacheKey = "AllMenus";
        public const string MenuTreeCacheKey = "AllMenusTree";

        private readonly Core.Models.SenparcEntities _senparcEntities;

        public SysMenuService(ClientRepositoryBase<SysMenu> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            _sysButtonService = _serviceProvider.GetService<SysButtonService>();
            _distributedCache = _serviceProvider.GetService<IDistributedCache>();
            _senparcEntities = _serviceProvider.GetService<Core.Models.SenparcEntities>();
        }

        /// <summary>
        /// TODO...重建菜单角色缓存
        /// </summary>
        /// <param name="sysMenuDto"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public async Task<SysMenu> CreateOrUpdateAsync(SysMenuDto sysMenuDto)
        {
            SysMenu menu;
            ICollection<SysButton> sysButtons = new List<SysButton>();
            if (!string.IsNullOrEmpty(sysMenuDto.Id))
            {
                menu = await GetObjectAsync(_ => _.Id == sysMenuDto.Id);
                menu.Update(sysMenuDto);
            }
            else
            {
                menu = new SysMenu(sysMenuDto);
            }
            await SaveObjectAsync(menu);
            await GetMenuDtoByCacheAsync(true);
            return menu;
        }

        /// <summary>
        /// TODO...重建菜单角色缓存
        /// </summary>
        /// <param name="sysMenuDto"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(SysMenuDto sysMenuDto, IEnumerable<SysButtonDto> buttons)
        {
            SysMenu menu;
            ICollection<SysButton> sysButtons = new List<SysButton>();
            if (!string.IsNullOrEmpty(sysMenuDto.Id))
            {
                menu = await GetObjectAsync(_ => _.Id == sysMenuDto.Id);
                menu.Update(sysMenuDto);
            }
            else
            {
                menu = new SysMenu(sysMenuDto);
            }

            IEnumerable<string> modifySysButtons = buttons.Where(_ => !string.IsNullOrEmpty(_.Id)).Select(_ => _.Id);
            IEnumerable<SysButton> updateBUttons;
            if (modifySysButtons.Any())
            {
                updateBUttons = await _sysButtonService.GetFullListAsync(_ => modifySysButtons.Contains(_.Id));
            }
            else
            {
                updateBUttons = new List<SysButton>();
            }
            foreach (var item in buttons)
            {
                if (string.IsNullOrEmpty(item.ButtonName))
                {
                    continue;
                }
                SysButton sysButton;
                if (string.IsNullOrEmpty(item.Id))
                {
                    sysButton = new SysButton(item);
                    sysButton.MenuId = menu.Id;
                }
                else
                {
                    sysButton = updateBUttons.FirstOrDefault(_ => _.Id == item.Id);
                    sysButton.Update(item);
                }
                sysButtons.Add(sysButton);
            }
            await BeginTransactionAsync(async () =>
            {
                if (sysButtons.Any())
                {
                    await _sysButtonService.SaveObjectListAsync(sysButtons);
                }
                await SaveObjectAsync(menu);
            });
            await GetMenuDtoByCacheAsync(true);
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task RemoveMenuAsync()
        {
            await _distributedCache.RemoveAsync(MenuCacheKey);
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysMenuDto>> GetMenuDtoByCacheAsync(bool isRefresh = false)
        {
            List<SysMenuDto> selectListItems = null;
            byte[] selectLiteItemBytes = await _distributedCache.GetAsync(MenuCacheKey);
            if (selectLiteItemBytes == null || isRefresh)
            {
                List<SysMenu> sysMenus = await GetFullListAsync(_ => true);
                List<SysButton> sysButtons = await _sysButtonService.GetFullListAsync(_ => true);
                selectListItems = Mapper.Map<List<SysMenuDto>>(sysMenus);
                List<SysMenuDto> buttons = _sysButtonService.Mapper.Map<List<SysMenuDto>>(sysButtons);
                selectListItems.AddRange(buttons);
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(selectListItems);
                await _distributedCache.RemoveAsync(MenuCacheKey);
                await _distributedCache.RemoveAsync(MenuTreeCacheKey);
                await _distributedCache.SetAsync(MenuCacheKey, System.Text.Encoding.UTF8.GetBytes(jsonStr));
                await _distributedCache.SetStringAsync(MenuTreeCacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(GetSysMenuTreesMainRecursive(selectListItems)));
            }
            else
            {
                selectListItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SysMenuDto>>(System.Text.Encoding.UTF8.GetString(selectLiteItemBytes));
            }
            return selectListItems;
        }

        public IEnumerable<SysMenuTreeItemDto> GetSysMenuTreesMainRecursive(IEnumerable<SysMenuDto> sysMenuTreeItems)
        {
            List<SysMenuTreeItemDto> sysMenuTrees = new List<SysMenuTreeItemDto>();
            getSysMenuTreesRecursive(sysMenuTreeItems, sysMenuTrees, null);
            return sysMenuTrees;
        }


        private void getSysMenuTreesRecursive(IEnumerable<SysMenuDto> sysMenuTreeItems, IList<SysMenuTreeItemDto> sysMenuTrees, string parentId)
        {
            foreach (var item in sysMenuTreeItems.Where(_ => _.ParentId == parentId && _.IsMenu))
            {
                SysMenuTreeItemDto sysMenu = new SysMenuTreeItemDto() { MenuName = item.MenuName, Id = item.Id, IsMenu = item.IsMenu, Icon = item.Icon, Url = item.Url, Children = new List<SysMenuTreeItemDto>() };
                sysMenuTrees.Add(sysMenu);
                getSysMenuTreesRecursive(sysMenuTreeItems, sysMenu.Children, item.Id);
            }
        }

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysMenuTreeItemDto>> GetMenuTreeDtoByCacheAsync()
        {
            IEnumerable<SysMenuTreeItemDto> sysMenuTreeItems = null;//
            string jsonStr = await _distributedCache.GetStringAsync(MenuTreeCacheKey);
            if (string.IsNullOrEmpty(jsonStr))
            {
                await GetMenuDtoByCacheAsync(true);
            }
            jsonStr = await _distributedCache.GetStringAsync(MenuTreeCacheKey);
            sysMenuTreeItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SysMenuTreeItemDto>>(jsonStr);
            return sysMenuTreeItems;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {

            var sql = @"BEGIN TRAN
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'91c13c73-1688-455f-94df-e8ecd1affdc7', 0, CAST(N'2019-11-04T15:15:14.9078474' AS DateTime2), CAST(N'2019-11-04T15:15:14.9078474' AS DateTime2), N'95d1dc86-52b5-4689-9735-a2c483f89e81', N'新增子级菜单', N'AddChild', N'/Admin/Menu/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'9ae3fbce-5768-4414-af3c-7a66b9e74a3a', 0, CAST(N'2019-11-04T15:13:23.9369838' AS DateTime2), CAST(N'2019-11-04T15:15:14.9078455' AS DateTime2), N'7c307710-45ca-46e2-a96e-7234c5fa4abd', N'保存', N'Save', N'/Admin/Menu/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'9cb64700-a0d5-4e35-8b89-cc921d93dfe2', 1, CAST(N'2019-11-04T15:10:24.3031650' AS DateTime2), CAST(N'2019-11-04T15:13:23.9369807' AS DateTime2), N'67855ef5-ab6b-4e93-978f-b618e5306005', N'新增', N'Add', N'/Admin/Role/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'a184c01e-d655-4b4a-a3d8-12dc751d2ad2', 0, CAST(N'2019-11-04T15:13:23.9369830' AS DateTime2), CAST(N'2019-11-04T15:13:23.9369830' AS DateTime2), N'7c307710-45ca-46e2-a96e-7234c5fa4abd', N'删除', N'Del', N'/Admin/Role/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'ab4b97cf-0394-419d-9c75-dbb04828e831', 0, CAST(N'2019-11-04T15:10:24.3029475' AS DateTime2), CAST(N'2019-11-04T15:10:24.3029475' AS DateTime2), N'67855ef5-ab6b-4e93-978f-b618e5306005', N'新增', N'Add', N'/Admin/AdminUserInfo/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'b1168466-fc2b-43ad-b449-297d5b1bf494', 0, CAST(N'2019-11-04T15:10:24.3031671' AS DateTime2), CAST(N'2019-11-04T15:10:24.3031671' AS DateTime2), N'67855ef5-ab6b-4e93-978f-b618e5306005', N'授权', N'Auth', N'/Admin/AdminUserInfo/AuthorizationPage', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'bd534d1f-fbdf-44f1-bd8a-9223d475ab07', 0, CAST(N'2019-11-04T15:10:24.3031667' AS DateTime2), CAST(N'2019-11-04T15:10:24.3031667' AS DateTime2), N'67855ef5-ab6b-4e93-978f-b618e5306005', N'编辑', N'Edit', N'/Admin/AdminUserInfo/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'e6fe98cf-4670-4665-b1c9-dbcd61a89f47', 0, CAST(N'2019-11-04T15:15:14.9078477' AS DateTime2), CAST(N'2019-11-04T15:15:14.9078477' AS DateTime2), N'95d1dc86-52b5-4689-9735-a2c483f89e81', N'删除', N'Del', N'/Admin/Menu/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'e9d3b568-cac5-4c1f-863d-f57117c215e8', 0, CAST(N'2019-11-04T15:15:14.9078469' AS DateTime2), CAST(N'2019-11-04T15:15:14.9078469' AS DateTime2), N'95d1dc86-52b5-4689-9735-a2c483f89e81', N'新增一级菜单', N'AddFirst', N'/Admin/Menu/Edit', NULL, NULL)
INSERT [dbo].[SysButtons] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuId], [ButtonName], [OpearMark], [Url], [AdminRemark], [Remark]) VALUES (N'f9c07719-97b9-4b30-9b01-6dcfae25813c', 0, CAST(N'2019-11-04T15:13:23.9369843' AS DateTime2), CAST(N'2019-11-04T15:13:23.9369843' AS DateTime2), N'7c307710-45ca-46e2-a96e-7234c5fa4abd', N'设置权限', N'Authentic', N'/Admin/Role/Permission', NULL, NULL)
INSERT [dbo].[SysMenus] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuName], [ParentId], [Url], [Icon], [Sort], [Visible], [AdminRemark], [Remark]) VALUES (N'538e9d87-e9da-4541-9269-13c3bc99bb9e', 0, CAST(N'2019-08-14T17:29:53.4776498' AS DateTime2), CAST(N'2019-11-04T15:21:31.0513206' AS DateTime2), N'系统管理', NULL, NULL, N'fa fa-cog', 0, 1, NULL, NULL)
INSERT [dbo].[SysMenus] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuName], [ParentId], [Url], [Icon], [Sort], [Visible], [AdminRemark], [Remark]) VALUES (N'67855ef5-ab6b-4e93-978f-b618e5306005', 0, CAST(N'2019-08-14T17:30:01.4833756' AS DateTime2), CAST(N'2019-11-04T15:10:24.4180192' AS DateTime2), N'管理员管理', N'538e9d87-e9da-4541-9269-13c3bc99bb9e', N'/Admin/AdminUserInfo/Index', N'fa fa-user-secret', 50, 1, NULL, NULL)
INSERT [dbo].[SysMenus] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuName], [ParentId], [Url], [Icon], [Sort], [Visible], [AdminRemark], [Remark]) VALUES (N'7c307710-45ca-46e2-a96e-7234c5fa4abd', 0, CAST(N'2019-08-14T17:30:08.3403569' AS DateTime2), CAST(N'2019-11-04T15:13:23.9880751' AS DateTime2), N'角色管理', N'538e9d87-e9da-4541-9269-13c3bc99bb9e', N'/Admin/Role/Index', N'fa fa-user', 49, 1, NULL, NULL)
INSERT [dbo].[SysMenus] ([Id], [Flag], [AddTime], [LastUpdateTime], [MenuName], [ParentId], [Url], [Icon], [Sort], [Visible], [AdminRemark], [Remark]) VALUES (N'95d1dc86-52b5-4689-9735-a2c483f89e81', 0, CAST(N'2019-08-14T17:30:14.1710351' AS DateTime2), CAST(N'2019-11-04T15:15:14.9146690' AS DateTime2), N'菜单管理', N'538e9d87-e9da-4541-9269-13c3bc99bb9e', N'/Admin/Menu/Index', N'fa fa-bars', 40, 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysPermission] ON 
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1094, 0, CAST(N'2019-11-05T09:53:41.9787383' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787384' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'e6fe98cf-4670-4665-b1c9-dbcd61a89f47', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1095, 0, CAST(N'2019-11-05T09:53:41.9787381' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787382' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'91c13c73-1688-455f-94df-e8ecd1affdc7', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1096, 0, CAST(N'2019-11-05T09:53:41.9787379' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787379' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 1, N'95d1dc86-52b5-4689-9735-a2c483f89e81', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1097, 0, CAST(N'2019-11-05T09:53:41.9787376' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787376' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'f9c07719-97b9-4b30-9b01-6dcfae25813c', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1098, 0, CAST(N'2019-11-05T09:53:41.9787373' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787374' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'a184c01e-d655-4b4a-a3d8-12dc751d2ad2', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1099, 0, CAST(N'2019-11-05T09:53:41.9787371' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787372' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'9ae3fbce-5768-4414-af3c-7a66b9e74a3a', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1100, 0, CAST(N'2019-11-05T09:53:41.9787368' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787369' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 1, N'7c307710-45ca-46e2-a96e-7234c5fa4abd', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1101, 0, CAST(N'2019-11-05T09:53:41.9787363' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787363' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'bd534d1f-fbdf-44f1-bd8a-9223d475ab07', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1102, 0, CAST(N'2019-11-05T09:53:41.9787360' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787361' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'b1168466-fc2b-43ad-b449-297d5b1bf494', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1103, 0, CAST(N'2019-11-05T09:53:41.9787358' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787358' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'ab4b97cf-0394-419d-9c75-dbb04828e831', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1104, 0, CAST(N'2019-11-05T09:53:41.9787355' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787356' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 1, N'67855ef5-ab6b-4e93-978f-b618e5306005', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1105, 0, CAST(N'2019-11-05T09:53:41.9787339' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787344' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 1, N'538e9d87-e9da-4541-9269-13c3bc99bb9e', NULL, NULL, NULL)
INSERT [dbo].[SysPermission] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [RoleId], [IsMenu], [PermissionId], [AdminRemark], [Remark], [ResourceCode]) VALUES (1106, 0, CAST(N'2019-11-05T09:53:41.9787386' AS DateTime2), CAST(N'2019-11-05T09:53:41.9787387' AS DateTime2), N'administrator', N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, N'e9d3b568-cac5-4c1f-863d-f57117c215e8', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysPermission] OFF
SET IDENTITY_INSERT [dbo].[SysRoleAdminUserInfos] ON 
INSERT [dbo].[SysRoleAdminUserInfos] ([Id], [Flag], [AddTime], [LastUpdateTime], [RoleCode], [AccountId], [RoleId], [AdminRemark], [Remark]) VALUES (4, 0, CAST(N'2019-08-15T11:51:01.2047663' AS DateTime2), CAST(N'2019-08-15T11:51:01.2047673' AS DateTime2), N'administrator', 1, N'1f0dcb10-c6cc-437c-ad0f-526debb18290', NULL, NULL)
SET IDENTITY_INSERT [dbo].[SysRoleAdminUserInfos] OFF
INSERT [dbo].[SysRoles] ([Id], [Flag], [AddTime], [LastUpdateTime], [Enabled], [RoleName], [RoleCode], [AdminRemark], [Remark]) VALUES (N'1f0dcb10-c6cc-437c-ad0f-526debb18290', 0, CAST(N'2019-08-14T17:28:47.1455467' AS DateTime2), CAST(N'2019-08-14T17:28:47.1455467' AS DateTime2), 1, N'超级管理员', N'administrator', NULL, NULL)
COMMIT";
            try
            {
                int affectRows = _senparcEntities.Database.ExecuteSqlRaw(sql);
            }
            catch (Exception ex)
            {

                throw new Exception("初始化数据失败，原因:" + ex);
            }
        }
    }
}
