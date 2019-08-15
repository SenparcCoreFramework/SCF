using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Service;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class AdminUserInfoAuthorizationPageModel : BaseAdminPageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SysRoleAdminUserInfoService sysRoleAdminUserInfoService;

        public AdminUserInfoAuthorizationPageModel(IServiceProvider _serviceProvider, SysRoleAdminUserInfoService sysRoleAdminUserInfoService)
        {
            this._serviceProvider = _serviceProvider;
            this.sysRoleAdminUserInfoService = sysRoleAdminUserInfoService;
        }

        public IEnumerable<SysRoleAdminUserInfoDto> RoleAdminUserInfoDtos { get; set; }

        public async Task OnGetAsync(int accountId)
        {
            AdminUserInfoService adminUserInfoService = _serviceProvider.GetService<AdminUserInfoService>();
            SysRoleService sysRoleService = _serviceProvider.GetService<SysRoleService>();
            //SysRoleAdminUserInfoService sysRoleAdminUserInfoService = _serviceProvider.GetService<SysRoleAdminUserInfoService>();

            IEnumerable<SysRole> sysRoles = await sysRoleService.GetFullListAsync(_ => true);
            IEnumerable<SysRoleAdminUserInfo> sysRoleAdminUserInfos = await sysRoleAdminUserInfoService.GetFullListAsync(_ => _.AccountId == accountId);

            IEnumerable<SysRoleAdminUserInfoDto> dto = from _ in sysRoles
                                                       join __ in sysRoleAdminUserInfos
                                                       on _.Id equals __.RoleId into __Left
                                                       from __ in __Left.DefaultIfEmpty()
                                                       select new SysRoleAdminUserInfoDto()
                                                       {
                                                           AdminAccountId = accountId,
                                                           HasRole = __ != null,
                                                           RoleId = _.Id,
                                                           RoleName = _.RoleName
                                                       };
            RoleAdminUserInfoDtos = dto;
        }

        public async Task<IActionResult> OnPostAsync(IEnumerable<string> RoleIds, int accountId)
        {
            if (!RoleIds.Any() || accountId <= 0)
            {
                return Ok(false);
            }
            await sysRoleAdminUserInfoService.AddAsync(RoleIds, accountId);
            return Ok(true);
        }
    }
}