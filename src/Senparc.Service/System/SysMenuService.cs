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

        public SysMenuService(ClientRepositoryBase<SysMenu> repo, SysButtonService sysButtonService, IDistributedCache distributedCache, IMapper mapper = null) : base(repo, mapper)
        {
            _sysButtonService = sysButtonService;
            _distributedCache = distributedCache;
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
            var transation = await BeginTransactionAsync();
            try
            {
                //await _sysButtonService.DeleteButtonsByMenuId(menu.Id);
                if (sysButtons.Any())
                {
                    await _sysButtonService.SaveObjectListAsync(sysButtons);
                }
                await SaveObjectAsync(menu);
                transation.Commit();
            }
            catch (Exception)
            {
                transation.Rollback();
                throw;
            }
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
    }
}
