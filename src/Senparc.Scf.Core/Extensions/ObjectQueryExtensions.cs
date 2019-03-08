using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Senparc.Scf.Core.Extensions
{
    public static class ObjectQueryExtensions
    {
        /// <summary>
        /// 添加多个Include()对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static IQueryable<T> Includes<T>(this IQueryable<T> obj, string[] includes) where T : class
        {
            //** 用法：entities.Users.Includes(new string[]{ "Roles","Products" }).ToList()
            if (includes == null)
            {
                return obj;
            }

            foreach (var item in includes)
            {
                obj = obj.Include(item);
            }
            return obj;
        }

        /// <summary>
        /// 添加多个可在编译时检测的Include类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includeTyles"></param>
        /// <returns></returns>
        public static IQueryable<T> Includes<T>(this IQueryable<T> obj, Type[] includeTyles) where T : class
        {
            //** 用法：entities.Users.Includes(new Type[]{ typeof(Roles),typeof(Products) }).ToList()
            if (includeTyles == null)
                return obj;

            foreach (var item in includeTyles)
            {
                obj = obj.Include(item);
            }
            return obj;
        }

        /// <summary>
        /// 添加可在编译时检测的Include类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="includeType"></param>
        /// <returns></returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> obj, Type includeType) where T : class
        {
            //** 用法：entities.Users.Include(typeof(Roles)).ToList()
            return obj.Include(includeType.Name);
        }

        /// <summary>
        /// 委托排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending">是否升序排列</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T, TK>(this IQueryable<T> obj, Expression<Func<T, TK>> orderBy, OrderingType orderingType) where T : class
        {
            if (orderBy == null)
                throw new Exception("OrderBy can not be Null！");

            return orderingType == OrderingType.Ascending ? obj.OrderBy(orderBy) : obj.OrderByDescending(orderBy);
        }

        /// <summary>
        /// 委托排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending">是否升序排列</param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderByIEnumerable<T, TK>(this IEnumerable<T> obj, Func<T, TK> orderBy, OrderingType orderingType) where T : class
        {
            if (orderBy == null)
                throw new Exception("OrderBy can not be Null！");

            return orderingType == OrderingType.Ascending ? obj.OrderBy(orderBy) : obj.OrderByDescending(orderBy);
        }

        //public static void TryLoad<T>(this EntityCollection<T> obj) where T : class, System.Data.Objects.DataClasses.IEntityWithRelationships
        //{
        //    if (!obj.IsLoaded) { obj.Load(); }
        //}

        //public static void TryLoad<T>(this EntityReference<T> obj) where T : class, System.Data.Objects.DataClasses.IEntityWithRelationships
        //{
        //    if (!obj.IsLoaded) { obj.Load(); }
        //}
    }
}
