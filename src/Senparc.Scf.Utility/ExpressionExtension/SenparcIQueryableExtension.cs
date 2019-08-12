using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.Utility.ExpressionExtension
{
    /// <summary>
    /// linq拓展方法
    /// </summary>
    public static class SenparcIQueryableExtension
    {
        const string OrderByDescendingFuncName = "OrderByDescending";

        const string OrderByFuncName = "OrderBy";

        const string ThenByFuncName = "ThenBy";

        const string ThenByDescendingFuncName = "ThenByDescending";

        const string Asc = "asc";

        const string Desc = "desc";

        /// <summary>
        /// 动态字段排序方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iQueryable"></param>
        /// <param name="orderBy">xxxx desc, aaa asc, ddd desc</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByExtension<T>(this IQueryable<T> iQueryable, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return iQueryable;
            }

            Type type = typeof(T);
            MethodCallExpression OrderExpression = null;
            string[] orderField = orderBy.Trim().Split(",");
            foreach (var item in orderField)
            {
                string[] singleField = item.Trim().Split(" ");
                string fieldName = singleField[0];
                string orderby = OrderByFuncName;

                ParameterExpression param = Expression.Parameter(type, "_");
                PropertyInfo property = type.GetProperty(fieldName);
                if (property == null)
                {
                    continue;
                }

                if (singleField.Length == 2)
                {
                    orderby = singleField[1].ToLower();
                }
                Expression propertyAccessExpression = Expression.Property(param, property);
                LambdaExpression le = Expression.Lambda(propertyAccessExpression, param);

                if (OrderExpression == null)
                {
                    string funcName = OrderByFuncName;
                    if (!string.IsNullOrEmpty(orderby) && orderby == Desc)
                    {
                        funcName = OrderByDescendingFuncName;
                    }
                    OrderExpression = Expression.Call(typeof(Queryable), funcName, new Type[] { type, property.PropertyType }, iQueryable.Expression, le);
                }
                else
                {
                    string funcName = ThenByFuncName;
                    if (!string.IsNullOrEmpty(orderby) && orderby == Desc)
                    {
                        funcName = ThenByDescendingFuncName;
                    }
                    OrderExpression = Expression.Call(typeof(Queryable), funcName, new Type[] { type, property.PropertyType }, OrderExpression, le);
                }

            }
            return iQueryable.Provider.CreateQuery<T>(OrderExpression);
        }
    }
}
