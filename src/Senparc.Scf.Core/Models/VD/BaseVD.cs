using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Senparc.Scf.Core.Models.VD
{
    public interface IBaseUiVD
    {
        string UserName { get; set; }

        bool IsAdmin { get; set; }

        string CurrentMenu { get; set; }

        List<Messager> MessagerList { get; set; }

        MetaCollection MetaCollection { get; set; }

        DateTime PageStartTime { get; set; }

        DateTime PageEndTime { get; set; }
    }


    public class Base_PagerVD
    {
        public int? PageIndex { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public Base_PagerVD(int? pageIndex, int pageCount, int totalCount)
        {
            PageIndex = pageIndex;
            PageCount = pageCount;
            TotalCount = totalCount;
        }
    }

    public class Base_AreaXmlVD
    {
        public List<AreaXML_Provinces> Provinces { get; set; }

        public List<AreaXML_Cities> Cities { get; set; }

        public List<AreaXML_Districts> Districts { get; set; }

        public string CurrentProvince { get; set; }

        public string CurrentCity { get; set; }

        public string CurrentDistrict { get; set; }
    }


    public class LoginBarVD
    {
        public bool Logined { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }



}