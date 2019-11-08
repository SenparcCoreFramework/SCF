using AutoMapper;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Respository;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Service
{
    public class SysButtonService : ClientServiceBase<SysButton>
    {
        private readonly ISysButtonRespository _iSysButtonRespository;

        public SysButtonService(ISysButtonRespository repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
            _iSysButtonRespository = repo;
        }

        public async Task<int> DeleteButtonsByMenuId(string menuId)
        {
            if (string.IsNullOrEmpty(menuId))
            {
                return 0;
            }
            return await _iSysButtonRespository.DeleteButtonsByMenuId(menuId);
        }

        public async Task<IEnumerable<SysButtonDto>> GetSysButtonDtosAsync(string MenuId)
        {
            IEnumerable<SysButton> sysButtons = await GetFullListAsync(_ => _.MenuId == MenuId);
            return Mapper.Map<IEnumerable<SysButtonDto>>(sysButtons);
        }
    }
}
