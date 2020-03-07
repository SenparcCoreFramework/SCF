using Senparc.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
using Senparc.ExtensionAreaTemplate.Respository;
using Senparc.Scf.Repository;
using Senparc.Scf.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Services
{
    public class ColorService : ServiceBase<Color>
    {
        public ColorService(IRepositoryBase<Color> repo, IServiceProvider serviceProvider)
            : base(repo, serviceProvider)
        {
        }

        public ColorDto CreateNewColor()
        {
            Color color = new Color(-1, -1, -1);
            base.SaveObject(color);
            ColorDto colorDto = base.Mapper.Map<ColorDto>(color);
            return colorDto;
        }

        //TODO: 更多业务方法可以写到这里
    }
}
