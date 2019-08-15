using AutoMapper;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Service
{
    public class SysRoleAdminUserInfoService : ClientServiceBase<SysRoleAdminUserInfo>
    {
        private readonly SysRoleService _sysRoleService;

        public SysRoleAdminUserInfoService(ClientRepositoryBase<SysRoleAdminUserInfo> repo, SysRoleService sysRoleService, IMapper mapper = null) : base(repo, mapper)
        {
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task AddAsync(IEnumerable<string> roleId, int accountId)
        {
            IList<SysRoleAdminUserInfo> sysRoleAdminUserInfos = new List<SysRoleAdminUserInfo>();

            IEnumerable<SysRole> sysRoles = await _sysRoleService.GetFullListAsync(_ => roleId.Contains(_.Id));
            foreach (var item in roleId)
            {
                SysRoleAdminUserInfo sysRoleAdminUserInfo = new SysRoleAdminUserInfo(accountId, item, sysRoles.FirstOrDefault(_ => _.Id == item)?.RoleCode);
                sysRoleAdminUserInfos.Add(sysRoleAdminUserInfo);
            }

            IEnumerable<SysRoleAdminUserInfo> sysRoleAdmins = await GetFullListAsync(_ => _.AccountId == accountId);

            await BeginTransactionAsync(async () => 
            {
                if (sysRoleAdmins.Any())
                {
                    await DeleteAllAsync(sysRoleAdmins);
                }
                if (sysRoleAdminUserInfos.Any())
                {
                    await SaveObjectListAsync(sysRoleAdminUserInfos);
                }
            });
        }
    }
}
