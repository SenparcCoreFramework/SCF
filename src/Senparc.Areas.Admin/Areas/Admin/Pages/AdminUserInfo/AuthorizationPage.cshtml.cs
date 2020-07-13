using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
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
            await Task.CompletedTask;
        }

        /// <summary>
        /// ��ȡ�û��Ľ�ɫ
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [Scf.AreaBase.Admin.Filters.CustomerResource("admin-grant")]
        public async Task<IActionResult> OnGetDetailAsync(int accountId)
        {
            IEnumerable<SysRoleAdminUserInfo> sysRoleAdminUserInfos = await sysRoleAdminUserInfoService.GetFullListAsync(_ => _.AccountId == accountId);
            return Ok(sysRoleAdminUserInfos);
        }

        [Scf.AreaBase.Admin.Filters.CustomerResource("admin-grant")]
        public async Task<IActionResult> OnPostAsync([FromBody] GrantRoleDto grantRoleDto)
        {
            await sysRoleAdminUserInfoService.AddAsync(grantRoleDto.RoleIds, grantRoleDto.AccountId);
            return Ok(true);
        }
    }

    public class GrantRoleDto
    {
        public IEnumerable<string> RoleIds { get; set; }

        public int AccountId { get; set; }
    }
}