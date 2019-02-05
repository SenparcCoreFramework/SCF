using System;
using System.Reflection;

namespace Senparc.Scf.Utility.Helpers
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            try
            {
                var ect = CreateInstance(assemblyName, nameSpace, className);
                return (T)ect;//强制转换类型
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static object CreateInstance(string assemblyName, string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className;//命名空间.类型名
                //此为第一种写法
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
                return ect;//返回
                //下面是第二种写法
                //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                //Type o = Type.GetType(path);//加载类型
                //object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                //return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回null
                return null;
            }
        }

        /// <summary>
        /// 根据程序集、命名空间、类名得到类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetTypeFromName(string assemblyName, string nameSpace, string className)
        {
            string fullName = nameSpace + "." + className;//命名空间.类型名
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            return o;
        }
    }
}