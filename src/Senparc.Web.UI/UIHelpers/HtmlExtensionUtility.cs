using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using Senparc.Scf.Core.Extensions;
using Microsoft.AspNetCore.Routing;
using Senparc.CO2NET.Extensions;

namespace System.Web.Mvc
{
    public static class HtmlExtensionUtility
    {

        #region Utility

        internal static bool AreEqual(object o1, object o2)
        {
            bool result = false;

            if (o1 == null && o2 == null)
            {
                result = true;
            }
            else
            {
                if (o1 != null)
                {
                    Type t = o1.GetType();
                    try
                    {
                        o2 = System.Convert.ChangeType(o2, t);
                        result = o1.Equals(o2);
                    }
                    catch
                    {
                        return false;
                    }
                }

            }

            return result;
        }

        internal static string EvalHashSetting(Hashtable hash, string key)
        {
            string result = string.Empty;
            if (hash.Keys.Count > 0)
            {
                if (hash.ContainsKey(key))
                {
                    return hash[key].ToString();
                }
            }
            return result;
        }


        //public static string RenderPage(System.Web.UI.Page pg)
        //{
        //    //render
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter writer = new StringWriter(sb);

        //    try
        //    {
        //        HttpContext.Current.Server.Execute(pg, writer, true);
        //    }
        //    catch (Exception x)
        //    {
        //        if (x.InnerException != null)
        //            throw x.InnerException;
        //        else
        //            throw x;
        //    }
        //    string result = sb.ToString();
        //    return result;

        //}


        /// <summary>
        /// Creates a formatted list of items based on the passed in format
        /// </summary>
        /// <param name="list">The item list</param>
        /// <param name="format">The single-place format string to use</param>
        public static string ToFormattedList(this IEnumerable list, string format)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator en = list.GetEnumerator();

            while (en.MoveNext())
            {
                sb.AppendFormat(format, en.Current.ToString());
            }
            return sb.ToString();

        }

        /// <summary>
        /// Finds a PropertyInfo item inside of a PropertyInfo[] Array regardless of case
        /// </summary>
        internal static PropertyInfo FindProp(string propName, PropertyInfo[] props)
        {
            PropertyInfo result = null;
            foreach (PropertyInfo p in props)
            {
                if (p.Name.ToLower().Trim().Equals(propName.ToLower().Trim()))
                {
                    result = p;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Sets the properties of an object based on a HashTable
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="settings"></param>
        internal static void SetPropsFromHash(object instance, Hashtable settings)
        {
            PropertyInfo pInfo = null;
            foreach (string key in settings.Keys)
            {

                //find a matching property
                pInfo = instance.GetType().GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (pInfo != null)
                {
                    pInfo.SetValue(instance, settings[key], null);
                }
            }
        }

        ///// <summary>
        ///// This is for utility only - used in the InstanceUserControl method
        ///// </summary>
        //internal class CustomPage : ViewPage
        //{
        //}

        /// <summary>
        /// Creates a simple {0}="{1}" list based on current object state.
        /// </summary>
        public static string ToAttributeList(this object o)
        {
            StringBuilder sb = new StringBuilder();
            if (o != null)
            {
                Hashtable attributeHash = GetPropertyHash(o);

                string resultFormat = " {0}=\"{1}\"";
                foreach (string attribute in attributeHash.Keys)
                {
                    sb.AppendFormat(resultFormat, attribute.Replace("_", ""), attributeHash[attribute]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a simple {0}='{1}';{3}={4} list based on current object state.
        /// </summary>
        public static string ToJsParams(this object o)
        {
            StringBuilder sb = new StringBuilder();
            if (o != null)
            {
                Hashtable attributeHash = GetPropertyHash(o);

                string stringFormat = "{0}=\'{1}\';";
                string numberFormat = "{0}={1};";
                string jsonFormat = "{0}={1};";

                foreach (string attribute in attributeHash.Keys)
                {
                    string formatStr = null;
                    string str = null;
                    var value = attributeHash[attribute];
                    var key = attribute.Replace("_", "");
                    if (value == null)
                    {
                        formatStr = jsonFormat;
                    }
                    else
                    {
                        var type = value.GetType();
                        if (type == typeof(Int16) ||
                            type == typeof(Int32) ||
                            type == typeof(Int64) ||
                            type == typeof(decimal)
                            )
                        {
                            formatStr = numberFormat;
                        }
                        else if (type == typeof(string[]))
                        {
                            formatStr = jsonFormat;
                            value = value.ToJson().Replace("\"", "'");
                        }
                        else if (type == typeof(bool))
                        {
                            str = $"{key}={((bool)value == true ? "true" : "false")};";
                        }
                        else
                        {
                            formatStr = stringFormat;
                        }
                    }

                    if (!formatStr.IsNullOrEmpty())
                    {
                        sb.AppendFormat(formatStr, key, value == null ? "null" : value.ToString());
                    }
                    else
                    {
                        sb.Append(str);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a simple {0}='{1}' list based on current object state. Ignores the passed-in string[] items
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignoreList"></param>
        /// <returns></returns>
        public static string ToAttributeList(this object o, params object[] ignoreList)
        {
            Hashtable attributeHash = GetPropertyHash(o);

            string resultFormat = "{0}=\"{1}\" ";
            StringBuilder sb = new StringBuilder();
            foreach (string attribute in attributeHash.Keys)
            {
                if (!ignoreList.Contains(attribute))
                {
                    sb.AppendFormat(resultFormat, attribute, attributeHash[attribute]);
                }
            }
            return sb.ToString();
        }


        public static string ConvertObjectToAttributeList(object value)
        {
            StringBuilder builder = new StringBuilder();
            if (value != null)
            {
                IDictionary<string, object> dictionary = value as IDictionary<string, object>;
                if (dictionary == null)
                {
                    dictionary = new RouteValueDictionary(value);
                }
                string format = "{0}=\"{1}\" ";
                foreach (string str2 in dictionary.Keys)
                {
                    object obj2 = dictionary[str2];
                    if (dictionary[str2] is bool)
                    {
                        obj2 = dictionary[str2].ToString().ToLowerInvariant();
                    }
                    builder.AppendFormat(format, str2.Replace("_", "").ToLowerInvariant(), obj2);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Creates a HashTable based on current object state
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static Hashtable GetPropertyHash(object properties)
        {

            Hashtable values = null;

            if (properties != null)
            {
                values = new Hashtable();

                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(properties);

                foreach (PropertyDescriptor prop in props)
                {
                    values.Add(prop.Name, prop.GetValue(properties));
                }
            }

            return values;
        }
        #endregion


    }
}
