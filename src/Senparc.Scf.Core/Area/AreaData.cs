using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.VD;
using Microsoft.AspNetCore.Http;

namespace Senparc.Scf.Core.Area
{
    /// <summary>
    /// 地区信息
    /// </summary>
    public class AreaData
    {
        private const string PROVINCES_XML_PATH = "~/App_Data/AreaData/Provinces.xml";
        private const string CITIES_XML_PATH = "~/App_Data/AreaData/Cities.xml";
        private const string DISTRICTS_XML_PATH = "~/App_Data/AreaData/Districts.xml";

        private XElement GetXmlElement(string xmlApplicationPath)
        {
            return XElement.Load(SenparcHttpContext.MapPath(xmlApplicationPath));
        }

        #region 省

        /// <summary>
        /// 获取所有省份数据（从缓存中获取）
        /// </summary>
        /// <returns></returns>
        public List<AreaXML_Provinces> GetProvincesData()
        {
            //检查缓存
            AreaDataCache_Province cacheData = new AreaDataCache_Province();

            if (cacheData.Data == null)
            {
                XElement doc = this.GetXmlElement(PROVINCES_XML_PATH);
                List<AreaXML_Provinces> dataList = (from p in doc.Elements()
                                                    //orderby p.Attribute("ID").Value
                                                    select new AreaXML_Provinces(int.Parse(p.Attribute("ID").Value),
                                                        p.Attribute("ProvinceName").Value,
                                                        p.Attribute("DivisionsCode").Value,
                                                        p.Attribute("ShortName").Value)
                ).ToList();

                cacheData.Data = dataList;
            }
            return cacheData.Data;
        }

        /// <summary>
        /// 获取单个省份信息
        /// </summary>
        /// <param name="provinceName"></param>
        /// <returns></returns>
        public AreaXML_Provinces GetProvinceData(string provinceName)
        {
            return GetProvincesData().FirstOrDefault(z => z.ProvinceName == provinceName);
        }
        /// <summary>
        /// 获取单个省份信息
        /// </summary>
        /// <param name="provinceID"></param>
        /// <returns></returns>
        public AreaXML_Provinces GetProvinceData(int provinceID)
        {
            return GetProvincesData().FirstOrDefault(z => z.ID == provinceID);
        }

        /// <summary>
        /// 获取指定CityName（城市）所属的省份信息
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public AreaXML_Provinces GetProvinceDataByCityName(string cityName)
        {
            var cityInfo = this.GetCityData(cityName);
            return GetProvincesData().FirstOrDefault(z => z.ID == cityInfo.PID);
        }

        #endregion


        #region 市
        /// <summary>
        /// 获取所有市区数据(从缓存中获取)
        /// </summary>
        /// <returns></returns>
        public List<AreaXML_Cities> GetCitiesData()
        {
            //检查缓存
            AreaDataCache_City cacheData = new AreaDataCache_City();
            if (cacheData.Data == null)
            {
                XElement doc = this.GetXmlElement(CITIES_XML_PATH);
                List<AreaXML_Cities> dataList = (from p in doc.Elements()
                                                 select new AreaXML_Cities(int.Parse(p.Attribute("ID").Value),
                                                     int.Parse(p.Attribute("PID").Value),
                                                     p.Attribute("CityName").Value,
                                                     p.Attribute("ZipCode").Value,
                                                     p.Attribute("CityCode").Value,
                                                    int.Parse(p.Attribute("MaxShopId").Value))
                ).ToList();

                cacheData.Data = dataList; //更新缓存
            }
            return cacheData.Data;
        }

        /// <summary>
        /// 获取指定PID城市数据
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public List<AreaXML_Cities> GetCitiesData(int pID)
        {
            List<AreaXML_Cities> fullCitiesData = GetCitiesData();

            return (from p in fullCitiesData
                    where (pID > 0 ? p.PID == pID : true)
                    select p).ToList();
        }

        /// <summary>
        /// 获取制定省份下面的所有市区名称
        /// </summary>
        /// <param name="provinceName"></param>
        /// <returns></returns>
        public List<AreaXML_Cities> GetCitiesData(string provinceName)
        {
            if (!string.IsNullOrEmpty(provinceName))
            {
                List<AreaXML_Provinces> fullProvincesData = GetProvincesData();
                var provinceData = fullProvincesData.Where(p => p.ProvinceName == provinceName).FirstOrDefault();// (from p in doc.Elements() where  select p.Attribute("PID").Value).First();

                if (provinceData != null)
                    return GetCitiesData(provinceData.ID);//省份存在，查询其下城市
                else
                    return new List<AreaXML_Cities>();
            }
            else
            {
                return new List<AreaXML_Cities>();
            }
        }


        private AreaXML_Cities GetCityData(string attributeName, string value)
        {
            List<AreaXML_Cities> fullCitiesData = GetCitiesData();

            AreaXML_Cities city = (from c in fullCitiesData
                                   where c.GetType().GetProperty(attributeName).GetValue(c, null).ToString() == value//应用到反射
                                   select c).FirstOrDefault();
            return city;
        }

        public AreaXML_Cities GetCityData(string cityName)
        {
            return this.GetCityData("CityName", cityName);
        }

        public AreaXML_Cities GetCityData(int cityCode)
        {
            return this.GetCityData("CityCode", cityCode.ToString());
        }

        public AreaXML_Cities GetCityDataById(int id)
        {
            return this.GetCityData("ID", id.ToString());
        }

        //public AreaXML_Cities GetCityData(string procinceName,string cityName)
        //{
        //    return this.GetCitiesData(procinceName).FirstOrDefault(z => z.CityName == cityName);
        //}


        /// <summary>
        /// 更新城市的区号、邮编
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="cityCode">区号</param>
        /// <param name="zipCode">邮编</param>
        /// <returns></returns>
        public bool UpdateCityCodeAndZipCode(string cityName, int cityCode, int zipCode)
        {
            try
            {
                //TODO:最好能应用单件模式
                XElement doc = this.GetXmlElement(CITIES_XML_PATH);//获取XML文档
                string xmlFilePath = SenparcHttpContext.MapPath(CITIES_XML_PATH);//路径
                string backUpXmlFilePath = xmlFilePath + "." + DateTime.Now.ToString().Replace(":", "_") + ".更新区号邮编（" + cityName + "）.bak";//备份文件路径

                var cityData = (from c in doc.Elements() where c.Attribute("CityName").Value == cityName select c).Single();
                //备份当前数据
                //File.Copy(xmlFilePath, backUpXmlFilePath);
                cityData.Save(backUpXmlFilePath);//备份单条信息

                cityData.SetAttributeValue("CityCode", cityCode.ToString());//更新区号
                cityData.SetAttributeValue("ZipCode", zipCode.ToString());//更新邮编

                //保存
                doc.Save(xmlFilePath);

                //更新缓存
                AreaDataCache_City areaData = new AreaDataCache_City();
                areaData.Data.Clear();//清空
                GetCitiesData();//使用调用，更新缓存

                return true;
            }
            catch { return false; }


        }

        /// <summary>
        /// 察看未定义邮编的城市
        /// </summary>
        /// <returns></returns>
        public List<AreaXML_Cities> GetWrongCityCode()
        {
            var cities = this.GetCitiesData();
            List<AreaXML_Cities> wrongCodeList = new List<AreaXML_Cities>();
            int outint = 0;
            foreach (var city in cities)
            {
                if (!int.TryParse(city.CityCode, out outint))
                {
                    wrongCodeList.Add(city);
                }
            }

            return wrongCodeList;
        }

        #endregion


        #region 区
        public List<AreaXML_Districts> GetDistrictsData()
        {
            //检查缓存
            AreaDataCache_District cacheData = new AreaDataCache_District();
            if (cacheData.Data == null)
            {
                XElement doc = this.GetXmlElement(DISTRICTS_XML_PATH);
                List<AreaXML_Districts> dataList = (from p in doc.Elements()
                                                    select new AreaXML_Districts(
                                                        int.Parse(p.Attribute("ID").Value),
                                                        int.Parse(p.Attribute("CID").Value),
                                                        p.Attribute("DistrictName").Value
                                                    )
                ).ToList();

                cacheData.Data = dataList;
            }
            return cacheData.Data;
        }

        /// <summary>
        /// 获取制定城市CID下面所有的区县数据
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public List<AreaXML_Districts> GetDistrictsData(int cID)
        {
            List<AreaXML_Districts> fullDictrictsData = GetDistrictsData();

            List<AreaXML_Districts> dictData = (from d in fullDictrictsData
                                                where (cID > 0 ? d.CID == cID : true)
                                                //orderby p.Attribute("ID")
                                                select d).ToList();

            //TODO:添加“其他区”，可以在XML中添加，但又似乎没有必要，再议。   ——2008.5.25 By TNT2
            if (dictData.Count > 0)
                dictData.Add(new AreaXML_Districts(-1, cID, "其他区"));

            return dictData;
        }

        /// <summary>
        /// 获取制定城市名称CityName下面所有的区县数据
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public List<AreaXML_Districts> GetDistrictsData(string cityName)
        {
            if (!string.IsNullOrEmpty(cityName))
            {
                List<AreaXML_Cities> fullCitiesData = GetCitiesData();
                AreaXML_Cities cityDta = fullCitiesData.Where(c => c.CityName == cityName).FirstOrDefault();

                if (cityDta != null)
                    return GetDistrictsData(cityDta.ID);
                else
                    return new List<AreaXML_Districts>();
            }
            else
            {
                return new List<AreaXML_Districts>();
            }
        }

        public AreaXML_Districts GetDistrictData(string cityName, string districtName)
        {
            return this.GetDistrictsData(cityName).FirstOrDefault(z => z.DistrictName == districtName);
        }

        #endregion

        /// <summary>
        /// 获取默认，或者用户所在地区的列表，不设置第一项默认项
        /// </summary>
        /// <param name="provinceName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public Base_AreaXmlVD GetAreaDataByProvinceAndCity(string provinceName, string cityName, string districtName)
        {
            return GetAreaDataByProvinceAndCity(provinceName, cityName, districtName, null, null, null);
        }


        /// <summary>
        /// 获取默认，或者用户所在地区的列表
        /// </summary>
        /// <param name="provinceName"></param>
        /// <param name="cityName"></param>
        /// <param name="TopProvince">Province第一项</param>
        /// <param name="TopCities">Cities第一项</param>
        /// <param name="TopDistricts">Districts第一项</param>
        /// <returns></returns>
        public Base_AreaXmlVD GetAreaDataByProvinceAndCity(string provinceName, string cityName, string districtName, AreaXML_Provinces TopProvince, AreaXML_Cities TopCities, AreaXML_Districts TopDistricts)
        {
            var vd = new Base_AreaXmlVD()
            {
                Provinces = this.GetProvincesData(),
                Cities = this.GetCitiesData(provinceName),
                Districts = this.GetDistrictsData(cityName),

                CurrentProvince = provinceName ?? "",
                CurrentCity = cityName ?? "",
                CurrentDistrict = districtName ?? ""
            };

            //加入第一行提示
            if (string.IsNullOrEmpty(provinceName))
            {
                vd.Cities = this.GetCitiesData("北京市");
                vd.Districts = this.GetDistrictsData("北京市");
            }

            //新增第一项
            if (TopProvince != null)
                vd.Provinces.Insert(0, TopProvince);

            if (TopCities != null)
                vd.Cities.Insert(0, TopCities);

            if (TopDistricts != null)
                vd.Districts.Insert(0, TopDistricts);


            return vd;
        }


        /// <summary>
        /// 增加区号属性
        /// </summary>
        //public void AddCityCodeAttrbuite()
        //{
        //    var doc = this.GetXmlElement(CITIES_XML_PATH);

        //    var cityList = doc.Elements();

        //    GLJKDataContext ctx=new GLJKDataContext();
        //    var codeList = ctx.AreaCityCodes.ToList();
        //    foreach (var item in cityList)
        //    {
        //        ////增加属性
        //        //item.SetAttributeValue("CityCode", "");
        //        //item.SetAttributeValue("MaxShopId", "100000");

        //        //城市名称配对
        //        //var citys = codeList.Where(s => item.CityName.Contains(s.City.Replace("自治区", "").Replace("县", "").Replace("市", ""))).ToList();


        //        var citys = codeList.Where(s => item.Attribute("CityName").Value.Contains(s.City.Replace("自治区","")));

        //        if (citys.Count() != 0)
        //        {
        //            var city = citys.Last();

        //            item.Attribute("CityCode").Value = city.CityCode;
        //        }
        //        else
        //        {
        //            //没有找到区号的城市
        //            System.Web.HttpContext.Current.Response.Write(
        //                "省："+this.GetProvincesData().Single(p=>p.ID==int.Parse(item.Attribute("PID").Value)).ProvinceName+" ,市："+ item.Attribute("CityName").Value+"<br />");
        //        }
        //    }
        //    doc.Save(System.Web.HttpContext.Current.Server.MapPath(CITIES_XML_PATH+".bak"));
        //}

        //public string GetCityCode(string provinceName, string cityName)
        //{
        //    var pro=GetAreaDataAll 
        //}


        /// <summary>
        /// 获取城市区号
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="format">是否格式化（前面加0）</param>
        /// <returns></returns>
        public string GetCityCode(/*string provinceName,*/ string cityName, bool format)
        {
            string cityCode = this.GetCityData(cityName).CityCode;
            return format ? "0" + cityCode : cityCode;//需要格式化，则在区号前加0
        }

        //#region ShopId相关

        ///// <summary>
        ///// 获取一个可用的ShopId
        ///// </summary>
        ///// <param name="cityName"></param>
        ///// <returns></returns>
        //public int GetUseableMaxShopId(string cityName)
        //{
        //    //获得Shop的XML设置
        //    var shopConfig = Config.XmlConfig.GetShopConfig();
        //    //获得当前ShopId
        //    var cityData = this.GetCityData(cityName);
        //    int currentMaxShopId = cityData.MaxShopId;//当前XML的MaxShopId
        //    int cityCode = int.Parse(cityData.CityCode);//区号
        //    int maxShopId = currentMaxShopId;

        //    //比较可用的ShopId
        //    bool findUseableShopId = false;
        //    while (!findUseableShopId)
        //    {
        //        if (shopConfig.ForbidShopIds.Contains(maxShopId.ToString()))
        //        {
        //            //存在于禁用ID列表中，退出，尝试下一个Id
        //            maxShopId++;
        //            continue;
        //        }
        //        else
        //        {
        //            //查找是否有相同ID存在
        //            GLJKDataContext ctx = new GLJKDataContext();
        //            if (ctx.Shop_GetShopByCityCodeAndShopId(cityCode, maxShopId) == null)
        //            {
        //                //ID可用
        //                findUseableShopId = true;
        //            }
        //            else
        //            {
        //                maxShopId++;
        //                continue;
        //            }
        //        }
        //    }

        //    return maxShopId;
        //}
        ///// <summary>
        ///// 更新XML中对应城市的MaxShopId
        ///// </summary>
        ///// <param name="cityName">城市名称</param>
        ///// <param name="newMaxShopId">当前使用掉的MaxShopId</param>
        ///// <returns></returns>
        //public bool UpdateMaxShopId(string cityName, int newMaxShopId)
        //{
        //    try
        //    {
        //        //TODO:最好能应用单件模式
        //        XElement doc = this.GetXmlElement(CITIES_XML_PATH);//获取XML文档
        //        string xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(CITIES_XML_PATH);//路径
        //        string backUpXmlFilePath = xmlFilePath + "." + DateTime.Now.ToString().Replace(":", "_") + ".更新MaxShopId（" + cityName + "）.bak";//备份文件路径

        //        var cityData = (from c in doc.Elements() where c.Attribute("CityName").Value == cityName select c).Single();
        //        //int shopId = int.Parse(cityData.Attribute("MaxShopId").Value);

        //        //备份当前数据
        //        //File.Copy(xmlFilePath, backUpXmlFilePath);
        //        cityData.Save(backUpXmlFilePath);//备份单条数据

        //        cityData.SetAttributeValue("MaxShopId", newMaxShopId + 1);
        //        //保存
        //        doc.Save(xmlFilePath);

        //        //更新缓存
        //        CacheData.AreaData_City areaData = new AreaDataCache_City();
        //        areaData.Clear();//清空
        //        GetCitiesData();//使用调用，更新缓存

        //        return true;
        //    }
        //    catch { return false; }
        //}




        //#endregion


        /// <summary>
        /// 通过IP数据的Country(省、市、区名称)，分割省、市、区信息以便查询
        /// </summary>
        /// <param name="ipCountry"></param>
        /// <param name="provinceName"></param>
        /// <param name="cityName">为null时连同不获取，</param>
        /// <param name="districtName">districtName不获取</param>
        public static void GetProvinceCityNameFromIPCountry(string ipCountry, ref string provinceName, ref string cityName, ref string districtName)
        {
            int areaStartIndex = 0;
            //省
            if (provinceName == null)
                return;
            else
                provinceName = GetProvinceAreaNameFromIPCountry(ipCountry, new string[] { "省", "自治区" }, ref areaStartIndex);

            //市
            if (cityName == null)
            {
                return;
            }
            else
            {
                if (ipCountry.IndexOf("市") != -1 && areaStartIndex == 0)
                    provinceName = cityName; //直辖市

                cityName = GetProvinceAreaNameFromIPCountry(ipCountry, new string[] { "市", "自治州" }, ref areaStartIndex);
            }

            //区
            if (districtName == null)
                return;
            else
                districtName = GetProvinceAreaNameFromIPCountry(ipCountry, new string[] { "区", "县", "市", "自治县" }, ref areaStartIndex);

        }
        /// <summary>
        /// 通过IP数据的Country(省、市、区名称)
        /// </summary>
        /// <param name="ipCountry">从IP数据库中得到的ipCountry字符串</param>
        /// <param name="areaNameList">地区级别称谓，如"省""市""区"</param>
        /// <param name="areaStartIndex">开始搜索字符串位置的游标</param>
        /// <returns></returns>
        private static string GetProvinceAreaNameFromIPCountry(string ipCountry, string[] areaNameList, ref int areaStartIndex)
        {
            string areaNameResult = string.Empty;
            foreach (var area in areaNameList)
            {
                if (ipCountry.IndexOf(area) != -1)
                {
                    areaNameResult = ipCountry.Substring(areaStartIndex, ipCountry.IndexOf(area) - areaStartIndex + area.Length);//获得地区名称
                    areaStartIndex += areaNameResult.Length;//字符串游标，搜索“区”的时候可忽略
                    break;
                }
            }
            return areaNameResult;
        }

    }
}
