using System;

namespace Senparc.Scf.Core.Utility
{
    public static class DateTimeUtility
    {
        public static long GetJavascriptTimestamp(System.DateTime input)
        {
            return input.AddTicks((-1) * DateTime.Parse("1970-1-1").Ticks).Ticks / 10000;
            //System.TimeSpan span = new System.TimeSpan(System.DateTime.Parse("1970-1-1").Ticks);
            //System.DateTime time =  input.Subtract(span);
            //return (int)((int)time.Ticks / 10000);
        }

        /// <summary>
        /// 获取正确可用的生日
        /// </summary>
        /// <returns></returns>
        public static DateTime GetUsableBirthday(int year, int month, int day)
        {
            year = Math.Min(DateTime.Now.Year, Math.Max(1921, year));//确保年在1921到今年之间
            month = Math.Min(12, Math.Max(1, month));//确保月份在1-12月之间
            day = Math.Min(31, Math.Max(1, day));//确保天在1-31日之间
            DateTime birthday;
            while (true)
            {
                if (DateTime.TryParse($"{year}-{month}-{day}", out birthday))
                {
                    break;
                }
                else
                {
                    day--;//如果日期不正确，则减一
                }
            }
            return birthday;
        }

        /// <summary>
        /// 获取本周六日期（周六为一周最后一天）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetStaturdayInWeek(DateTime date)
        {
            DateTime staturday = date.Date;
            while (staturday.DayOfWeek != DayOfWeek.Saturday)
            {
                staturday = staturday.AddDays(1);
            }
            return staturday;
        }

        /// <summary>
        /// 获取本周日日期（周日为一周的第一天）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSundayInWeek(DateTime date)
        {
            DateTime sunday = date.Date;
            while (sunday.DayOfWeek != DayOfWeek.Sunday)
            {
                sunday = sunday.AddDays(-1);
            }
            return sunday;
        }

        /// <summary>
        /// 获取当天是本年度第几周
        /// </summary>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime date)
        {
            var firstDay = new DateTime(date.Year, 1, 1);
            int firstWeekendDayOfYear = 7 - (int)firstDay.DayOfWeek;//第一个周末（周六）是几号
            int lastWeek = (date.DayOfYear - firstWeekendDayOfYear + 6) / 7;
            return lastWeek + 1;
        }

        /// <summary>
        /// 获取指定周的第一天（周日）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSundayOfWeek(int year, int weeek)
        {
            var firstDay = new DateTime(year, 1, 1);
            int firstWeekend = 7 - (int)firstDay.DayOfWeek;//第一个周末（周六）是几号
            int dayOfYear = firstWeekend + (weeek - 2) * 7;
            return firstDay.AddDays(dayOfYear);
        }
    }
}