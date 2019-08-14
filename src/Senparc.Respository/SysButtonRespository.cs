using Microsoft.EntityFrameworkCore;
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Respository
{
    public interface ISysButtonRespository : IClientRepositoryBase<SysButton>
    {
        /// <summary>
        /// 删除某个页面下的所有按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<int> DeleteButtonsByMenuId(string menuId);
    }

    public class SysButtonRespository : ClientRepositoryBase<SysButton>, ISysButtonRespository
    {
        private readonly SenparcEntities _senparcEntities;

        public SysButtonRespository(ISqlBaseFinanceData db) : base(db)
        {
            _senparcEntities = db.BaseDataContext as SenparcEntities;
        }

        /// <summary>
        /// 删除某个页面下的所有按钮
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<int> DeleteButtonsByMenuId(string menuId)
        {
            return await _senparcEntities.Database.ExecuteSqlCommandAsync($"DELETE {nameof(_senparcEntities.SysButtons)} WHERE {nameof(SysButton.MenuId)} = {{0}}", menuId);
        }
    }
}
