using Senparc.Scf.Core.Models;
using System;

namespace Senparc.Scf.Core.Models.VD
{
    /// <summary>
    /// 地区数据（省、市、区）
    /// </summary>
    public class Common_AreaListVD : Base_AreaXmlVD
    {
        /// <summary>
        /// ID,Name前缀
        /// </summary>
        public string Prefixes { get; set; }

        public string UserProvince { get; set; }

        public string UserCity { get; set; }

        public string UserDistrict { get; set; }

        public AreaXML_Provinces TopProvince { get; set; }

        public AreaXML_Cities TopCity { get; set; }

        public AreaXML_Districts TopDistrict { get; set; }

        public bool HideDistrict { get; set; }

        public bool ShowProvinceAllButton { get; set; }

        public bool ShowCityAllButton { get; set; }

        public bool ShowDistrictAllButton { get; set; }

        public Common_AreaListVD() { }

        public Common_AreaListVD(Base_AreaXmlVD areaInfo, string userProvince, string userCity, string userDistrict)
            : this(null, areaInfo, userProvince, userCity, userDistrict)
        { }

        public Common_AreaListVD(string prefixes, Base_AreaXmlVD areaInfo, string userProvince, string userCity, string userDistrict, AreaXML_Provinces topProvince = null, AreaXML_Cities topCity = null, AreaXML_Districts topDistrict = null, bool hideDistrict = false, bool showProvinceAllButton = false, bool showCityAllButton = false)
        {
            Prefixes = prefixes;

            Provinces = areaInfo.Provinces;
            Cities = areaInfo.Cities;
            Districts = areaInfo.Districts;

            UserProvince = userProvince;
            UserCity = userCity;
            UserDistrict = userDistrict;

            //如果首项为空，则ID设为-1
            topProvince = topProvince ?? new AreaXML_Provinces(-1, "", "", "");
            topCity = topCity ?? new AreaXML_Cities(-1, 0, "", "", "", 0);
            topDistrict = topDistrict ?? new AreaXML_Districts(-1, 0, "");

            TopProvince = topProvince;
            TopCity = topCity;
            TopDistrict = topDistrict;

            HideDistrict = hideDistrict;
            ShowProvinceAllButton = showProvinceAllButton;
            ShowCityAllButton = showCityAllButton;
        }
    }


    public class Common_AccountTypeListVD
    {
        public decimal AgentAreaRate { get; set; }

        public int AccountId { get; set; }

        //public List<FullAccountType> UserTypeList { get; set; }

        public int AccountTypeId { get; set; }

        public decimal AccountTypePrice { get; set; }

        public DateTime AccountTypeExpireTime { get; set; }

        public bool ShowUpgradeButton { get; set; }

        public bool AccountTypeOnOffer { get; set; }

        public string PostAction { get; set; }

        public string PostController { get; set; }

        public int MaxAppCount { get; set; }
    }
}