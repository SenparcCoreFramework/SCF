using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Utilities;
using Senparc.Scf.Core.Extensions;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Senparc.Scf.Core.Utility
{
    public partial class XmlDataContext
    {
        public string DatabaseDictionary { get; set; }

        //private const string RESET_PASSWORD_CODE = "ResetPasswordCode.xml";

        /// <summary>
        /// 文件名，不包含扩展名
        /// </summary>
        public string FileName { get; set; }

        public XmlDataContext()
            : this(null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseDictionary">数据库路径，必须是~/开头</param>
        /// <param name="fileName"></param>
        public XmlDataContext(string databaseDictionary, string fileName = null)
        {
            DatabaseDictionary = string.IsNullOrEmpty(databaseDictionary) ? "~/App_Data/DataBase/" : databaseDictionary;
            if (!DatabaseDictionary.EndsWith("/"))
            {
                DatabaseDictionary += "/";
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                FileName = fileName;
            }
        }

        /// <summary>
        /// 载入xml文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private XElement GetXElement(string path)
        {
            return XElement.Load(this.GetMapPath(path));//path.Replace("~/", HttpRuntime.AppDomainAppPath));
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetMapPath(string path)
        {
            return ServerUtility.ContentRootMapPath(path);//path.Replace("~/", HttpRuntime.AppDomainAppPath);// _context.Server.MapPath(path);
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetXmlFullApplicationPath(string entityName)
        {
            var fileName = FileName.IsNullOrEmpty() ? entityName : FileName;
            return DatabaseDictionary + fileName + ".config";//xml文件规则为 “{实例名称}.config”
        }

        /// <summary>
        /// 获取xml数据，并返回TEntity实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> GetXmlList<TEntity>() where TEntity : class, new()
        {
            string entityName = typeof(TEntity).Name;

            XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));
            List<TEntity> results = new List<TEntity>();

            foreach (var x in xml.Elements(entityName))
            {
                TEntity result = ConvertXmlToEntity<TEntity>(x);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// 将Xml Item节点内容专为实体类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        private TEntity ConvertXmlToEntity<TEntity>(XElement x) where TEntity : class, new()
        {
            TEntity result = new TEntity();
            foreach (var prop in result.GetType().GetProperties())
            {
                string value = x.Element(prop.Name).Value;
                //prop.SetValue(result, value, null);
                switch (prop.PropertyType.Name)
                {
                    case "DateTime":
                        prop.SetValue(result, DateTime.Parse(value), null);
                        break;
                    case "Int32":
                        prop.SetValue(result, int.Parse(value), null);
                        break;
                    case "Int64":
                        prop.SetValue(result, long.Parse(value), null);
                        break;
                    case "Single":
                    case "float":
                        prop.SetValue(result, float.Parse(value), null);
                        break;
                    case "Double":
                        prop.SetValue(result, double.Parse(value), null);
                        break;
                    case "Boolean":
                        prop.SetValue(result, bool.Parse(value), null);
                        break;
                    default:
                        //判断是否为枚举类型
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(result, Enum.Parse(prop.PropertyType, value), null);
                        }
                        else
                        {
                            prop.SetValue(result, value, null);
                        }
                        break;
                }
            }
            return result;
        }

        public List<AutoSendEmail> GetAutoSendEmailList()
        {
            return this.GetXmlList<AutoSendEmail>();
        }

        public List<AutoSendEmailBak> GetAutoSendEmailBakList()
        {
            return this.GetXmlList<AutoSendEmailBak>();
        }

        ///// <summary>
        ///// 重设密码
        ///// </summary>
        //public List<ResetPasswordCode> ResetPasswordCodes
        //{
        //    get
        //    {
        //        return this.GetXmlList<ResetPasswordCode>();
        //    }
        //}
    }


    public partial class XmlDataContext
    {
        //public bool Save<TEntity>(IEnumerable<TEntity> entityList) where TEntity : class
        //{
        //    try
        //    {
        //        string entityName = typeof(TEntity).Name;
        //        XElement xml = new XElement(entityName + "s");
        //        var props = typeof(TEntity).GetProperties();

        //        foreach (var entity in entityList)
        //        {
        //            XElement xe = new XElement(entityName);

        //            foreach (var prop in props)
        //            {
        //                switch (prop.PropertyType.Name)
        //                {
        //                    case "DateTime":
        //                        xe.Add(new XElement(prop.Name, prop.GetValue(entity, null).ToString()));
        //                        break;
        //                    default:
        //                        xe.Add(new XElement(prop.Name, prop.GetValue(entity, null)));
        //                        break;
        //                }
        //            }

        //            xml.Add(xe);
        //        }

        //        xml.Save(GetMapPath(GetXmlFullApplicationPath(entityName)));

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 保存记录，需要有ID
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Save<TEntity>(IEnumerable<TEntity> entitys) where TEntity : class
        {
            try
            {
                string entityName = typeof(TEntity).Name;
                XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));

                var props = typeof(TEntity).GetProperties();
                var idProp = typeof(TEntity).GetProperty("Id");
                if (idProp == null)
                {
                    throw new Exception("Id属性不存在！");
                }

                foreach (var entity in entitys)
                {
                    int id = (int)idProp.GetValue(entity, null);

                    //查找原纪录
                    XElement oldElement = xml.Elements(entityName).FirstOrDefault(z => z.Element("Id").Value == id.ToString());
                    if (oldElement == null)
                    {
                        throw new Exception("待更新数据不存在！");
                    }

                    //开始更新
                    foreach (var prop in props)
                    {
                        if (prop.Name == "Id")
                        {
                            continue;
                        }
                        XElement valueElement = oldElement.Element(prop.Name);
                        if (valueElement == null)
                        {
                            continue;
                        }

                        switch (prop.PropertyType.Name)
                        {
                            case "DateTime":
                            case "Int32":
                                valueElement.Value = prop.GetValue(entity, null).ToString();
                                break;
                            default:
                                valueElement.RemoveAll();
                                valueElement.Add(new XCData(prop.GetValue(entity, null).ToString()));
                                break;
                        }
                    }
                }

                //保存更新
                xml.Save(this.GetMapPath(this.GetXmlFullApplicationPath(entityName)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                string entityName = typeof(TEntity).Name;
                XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));

                //判断主键
                int maxId = -1;
                XAttribute maxIdAttr = xml.Attribute("maxId");
                if (maxIdAttr != null)
                {
                    maxId = Convert.ToInt32(maxIdAttr.Value);
                    maxId++;
                    maxIdAttr.Value = maxId.ToString();

                }

                var props = typeof(TEntity).GetProperties();
                XElement xe = new XElement(entityName);
                foreach (var prop in props)
                {
                    string name = prop.Name;
                    object value = prop.GetValue(entity, null).ToString();
                    if (maxId >= 0 && name == "Id")
                    {
                        value = maxId;
                        maxIdAttr.Value = maxId.ToString();
                        prop.SetValue(entity, maxId, null); //把最新的Id设置到实体
                    }

                    switch (prop.PropertyType.Name)
                    {
                        case "DateTime":
                        case "Int32":
                            xe.Add(new XElement(name, value));
                            break;
                        default:
                            xe.Add(new XElement(name, new XCData(value.ToString())));
                            break;
                    }
                }
                xml.Add(xe);
                xml.Save(this.GetMapPath(this.GetXmlFullApplicationPath(entityName)));
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 使用此方法须确定数据库中没有重复项（或有主键）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                string entityName = typeof(TEntity).Name;
                XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));//载入文档

                var idProp = typeof(TEntity).GetProperty("Id");
                if (idProp == null)
                {
                    throw new Exception("Id属性不存在！");
                }

                int id = (int)idProp.GetValue(entity, null);
                //查找原纪录
                XElement delElement = xml.Elements(entityName).FirstOrDefault(z => z.Element("Id").Value == id.ToString());
                if (delElement == null)
                {
                    return true;//throw new Exception("待更新数据不存在！");
                }
                delElement.Remove();//删除节点

                xml.Save(this.GetMapPath(this.GetXmlFullApplicationPath(entityName)));
                return true;
            }
            catch //(Exception e)
            {
                //XmlDataContext ctx = new XmlDataContext(_context);
                //AutoSendEmail error = new AutoSendEmail
                //{
                //    Subject = "发送出错记录",
                //    Body = e.Message + "\r\n" + e.StackTrace,
                //    LastSendTime = DateTime.Now,
                //    UserName = "System",
                //    SendCount = 505,
                //    Address = "szw2003@163.com",
                //};
                //ctx.Insert(error);

                return false;
            }
        }

        /// <summary>
        /// 使用此方法须确定数据库中没有重复项（或有主键）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity GetItem<TEntity>(object id) where TEntity : class, new()
        {
            try
            {
                string entityName = typeof(TEntity).Name;
                XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));//载入文档

                var idProp = typeof(TEntity).GetProperty("Id");
                if (idProp == null)
                {
                    throw new Exception("Id属性不存在！");
                }

                //查找原纪录
                XElement item = xml.Elements(entityName).FirstOrDefault(z => z.Element("Id").Value == id.ToString());
                if (item == null)
                {
                    return null;//throw new Exception("待更新数据不存在！");
                }

                TEntity result = ConvertXmlToEntity<TEntity>(item);
                return result;
            }
            catch //(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 仅保留指定项目
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void RetainItems<TEntity>(int retainItemCount) where TEntity : class, new()
        {
            string entityName = typeof(TEntity).Name;
            XElement xml = this.GetXElement(GetXmlFullApplicationPath(entityName));//载入文档

            var elements = xml.Elements(entityName);
            if (elements.Count() >= retainItemCount)
            {
                elements.Take(elements.Count() - retainItemCount).Remove();
            }
            xml.Save(this.GetMapPath(this.GetXmlFullApplicationPath(entityName)));
        }

        /// <summary>
        /// 尝试创建数据库
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public void TryCreateDataBase<TEntity>()
        {
            string entityName = typeof(TEntity).Name;

            //判断文件是否存在，如果不存在则新建
            var filePath = GetXmlFullApplicationPath(entityName);
            if (!File.Exists(filePath))
            {
                var xml = new XElement(entityName + "s", new XAttribute("maxId", 0));
                xml.Save(this.GetMapPath(this.GetXmlFullApplicationPath(entityName)));
            }
        }
    }


    #region XML DataBase格式

    ///// <summary>
    ///// 重设密码
    ///// </summary>
    //public partial class ResetPasswordCode
    //{
    //    public string UserName { get; set; }
    //    public string Code { get; set; }
    //    public DateTime CreateTime { get; set; }
    //}

    #endregion
}