using System;
using System.Collections.Generic;
using System.Reflection;

namespace Senparc.Scf.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntitySetKeys
    {
        public static EntitySetKeysDictionary Keys = new EntitySetKeysDictionary();
    }


    /// <summary>
    /// 与ORM实体类对应的实体集
    /// </summary>
    public class EntitySetKeysDictionary : Dictionary<Type, string>
    {
        public EntitySetKeysDictionary()
        {
            //初始化的时候从ORM中自动读取实体集名称及实体类别名称
            var clientProperties = typeof(Models.SenparcEntities).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            var properities = new List<PropertyInfo>();
            properities.AddRange(clientProperties);

            foreach (var prop in properities)
            {
                try
                {
                    //ObjectQuery，ObjectSet for EF4，DbSet for EF Code First
                    if (prop.PropertyType.Name.IndexOf("DbSet") != -1 && prop.PropertyType.GetGenericArguments().Length > 0)
                    {
                        this[prop.PropertyType.GetGenericArguments()[0]] = prop.Name;//获取第一个泛型
                    }
                }
                catch
                {
                }
            }
        }

        public new string this[Type entityType]
        {
            get
            {
                if (!base.ContainsKey(entityType))
                {
                    throw new Exception("未找到实体类型");
                }
                return base[entityType];
            }
            set
            {
                base[entityType] = value;
            }
        }
    }
}