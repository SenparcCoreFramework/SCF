using Microsoft.EntityFrameworkCore;
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
        private static EntitySetKeysDictionary Keys = new EntitySetKeysDictionary();

        internal static List<Type> DbContextStore { get; set; } = new List<Type>();

        internal static object DbContextStoreLock = new object();

        public static EntitySetKeysDictionary GetEntitySetKeys(Type tryLoadDbContextType)
        {
            return Keys.GetKeys(tryLoadDbContextType);
        }
    }


    /// <summary>
    /// 与ORM实体类对应的实体集
    /// </summary>
    public class EntitySetKeysDictionary : Dictionary<Type, string>
    {
        public EntitySetKeysDictionary GetKeys(Type tryLoadDbContextType)
        {
            //if (tryLoadDbContextType)
            //{
            //}
            //TODO:判断必须是是DbContext类型

            lock (EntitySetKeys.DbContextStoreLock)
            {
                if (!tryLoadDbContextType.IsSubclassOf(typeof(DbContext) ))
                {
                    throw new ArgumentException($"{nameof(tryLoadDbContextType)}不是 DbContext 的子类！", nameof(tryLoadDbContextType));
                }

                if (EntitySetKeys.DbContextStore.Contains(tryLoadDbContextType))
                {
                    return this;
                }
                EntitySetKeys.DbContextStore.Add(tryLoadDbContextType);


                //初始化的时候从ORM中自动读取实体集名称及实体类别名称
                var clientProperties = tryLoadDbContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

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

            return this;
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