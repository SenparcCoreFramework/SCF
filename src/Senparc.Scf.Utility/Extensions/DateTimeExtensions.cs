//using System;

//namespace Senparc.Scf.Core.Extensions
//{
//    public static class DateTimeExtensions
//    {
//        public static string ToShortTime(this DateTime dt)
//        {
//            string result = null;
//            TimeSpan timeSpan = DateTime.Now - dt;
//            if (timeSpan.TotalSeconds < 59)
//            {
//                result = "刚刚";
//            }
//            else if (timeSpan.TotalMinutes < 59)
//            {
//                result = "{0}分钟前".With((int)timeSpan.TotalMinutes);
//            }
//            else if (timeSpan.TotalHours < 24)
//            {
//                result = "{0}小时前".With((int)timeSpan.TotalHours);
//            }
//            else
//            {
//                result = "{0}天前".With((int)timeSpan.TotalDays);
//            }
//            return result;
//        }

//    }
//}
