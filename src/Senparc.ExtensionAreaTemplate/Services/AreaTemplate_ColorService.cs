using Senparc.Scf.Repository;
using Senparc.Scf.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Services
{
    public class AreaTemplate_ColorService : ServiceBase<AreaTemplate_Color>
    {
        public AreaTemplate_ColorService(IRepositoryBase<AreaTemplate_Color> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
        }

        //TODO: 更多业务方法可以写到这里
    }
}
