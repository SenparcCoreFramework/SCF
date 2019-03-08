using AutoMapper;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.AutoMapper
{
    /// <summary>
    /// AutoMapp 的 Profile
    /// </summary>
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            //CreateMap<CreateOrUpdate_AdminUserInfoDto, AdminUserInfo>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
