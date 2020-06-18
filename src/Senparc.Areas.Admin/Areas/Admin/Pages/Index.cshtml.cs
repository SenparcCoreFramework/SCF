using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Service;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Service;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Senparc.Areas.Admin.Pages
{
    public class IndexModel : BaseAdminPageModel
    {
        public IActionResult OnGet()
        {
            return null;
            //return RedirectToPage("/Home/Index");
        }

        public async Task<IActionResult> OnGetMenuTreeAsync([FromServices] SenparcEntities senparcEntities)
        {
            IEnumerable<SysMenuDto> sysMenus = await senparcEntities.SysMenus
                .Where(_ => _.Visible)
                .OrderByDescending(_ => _.Sort)
                .Select(_ => new SysMenuDto()
                {
                    MenuName = _.MenuName,
                    Id = _.Id,
                    Icon = _.Icon,
                    ParentId = _.ParentId,
                    ResourceCode = string.Empty,
                    Url = _.Url,
                    IsMenu = true,

                })
                .ToListAsync();
            var results = getSysMenuTreesMainRecursive(sysMenus);
            return Ok(results);
        }

        private IEnumerable<SysMenuTreeItemDto> getSysMenuTreesMainRecursive(IEnumerable<SysMenuDto> sysMenuTreeItems)
        {
            bool hideModuleManager = FullSystemConfig.HideModuleManager == true;
            List<SysMenuTreeItemDto> sysMenuTrees = new List<SysMenuTreeItemDto>();
            List<SysMenuDto> dest = new List<SysMenuDto>();
            int index = 60000;
            foreach (var item in sysMenuTreeItems)
            {
                Scf.XscfBase.IXscfRegister xscfRegister = XscfRegisterList.FirstOrDefault(z => !string.IsNullOrEmpty(item.Url) && item.Url.Contains($"uid={z.Uid}", StringComparison.OrdinalIgnoreCase));
                if (xscfRegister != null && xscfRegister is Senparc.Scf.Core.Areas.IAreaRegister xscfAreapage && xscfAreapage.AareaPageMenuItems.Count() > 0)
                {
                    if (hideModuleManager)
                    {
                        item.ParentId = null;
                        item.Id = (index++).ToString();
                    }
                    dest.Add(new SysMenuDto()
                    {
                        MenuName = "设置/执行",
                        Url = item.Url,
                        Id = (index++).ToString(),
                        ParentId = item.Id,
                        IsMenu = true
                    });
                    dest.AddRange(xscfAreapage.AareaPageMenuItems.Select(_ => new SysMenuDto()
                    {
                        MenuName = _.Name,
                        Url = _.Url,
                        Icon = _.Icon,
                        Id = (index++).ToString(),
                        ParentId = item.Id,
                        IsMenu = true
                    }));
                    item.Url = string.Empty;
                }
                if (hideModuleManager && item.Id == "4")
                {
                    continue;
                }//拓展模块
                dest.Add(item);
            }
            getSysMenuTreesRecursive(dest, sysMenuTrees, null);
            return sysMenuTrees;
        }
        private void getSysMenuTreesRecursive(IEnumerable<SysMenuDto> sysMenuTreeItems, IList<SysMenuTreeItemDto> sysMenuTrees, SysMenuDto parent)
        {
            IEnumerable<SysMenuDto> list = sysMenuTreeItems.Where(_ => _.ParentId == parent?.Id && _.IsMenu);
            foreach (var item in list)
            {
                SysMenuTreeItemDto sysMenu = new SysMenuTreeItemDto() { MenuName = item.MenuName, Id = item.Id, IsMenu = item.IsMenu, Icon = item.Icon, Url = item.Url, Children = new List<SysMenuTreeItemDto>() };
                sysMenuTrees.Add(sysMenu);
                getSysMenuTreesRecursive(sysMenuTreeItems, sysMenu.Children, item);
            }

        }
    }
}