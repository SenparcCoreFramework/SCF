using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.AutoMapProfile
{
    /// <summary>
    /// AutoMap 映射配置关系
    /// </summary>
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {

            CreateMap<SysMenu, SelectListItem>().ForMember(_ => _.Text, opt => opt.MapFrom(_ => _.MenuName))
                .ForMember(_ => _.Value, opt => opt.MapFrom(_ => _.Id));
            CreateMap<SysPermission, SysPermissionDto>();
            CreateMap<SysMenu, SysMenuDto>().ForMember(_ => _.IsMenu, opt => opt.MapFrom(_ => true)).ForMember(_ => _.ResourceCode, opt => opt.Ignore());
            CreateMap<SysButton, SysMenuDto>().ForMember(_ => _.IsMenu, opt => opt.MapFrom(_ => false))
                .ForMember(_ => _.MenuName, opt => opt.MapFrom(_ => _.ButtonName))
                .ForMember(_ => _.ParentId, opt => opt.MapFrom(_ => _.MenuId))
                .ForMember(_ => _.Sort, opt => opt.MapFrom(_ => 0))
                .ForMember(_ => _.Url, opt => opt.MapFrom(_ => _.Url))
                .ForMember(_ => _.Visible, opt => opt.MapFrom(_ => true))
                .ForMember(_ => _.Id, opt => opt.MapFrom(_ => _.Id))
                .ForMember(_ => _.ResourceCode, opt => opt.MapFrom(_ => _.OpearMark));
        }
    }
}
