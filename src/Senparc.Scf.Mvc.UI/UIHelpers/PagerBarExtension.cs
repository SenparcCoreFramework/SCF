using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Senparc.CO2NET;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Utility;
using System.Text;

namespace System.Web.Mvc
{
    public enum BarStyle
    {
        defaultStyle = 0,//默认
        digg,
        yahoo,
        meneame,
        flickr,
        sabrosus,
        scott,
        quotes,
        black,
        black2,
        black_red,
        grayr,
        yellow,
        jogger,
        starcraft2,
        tres,
        megas512,
        technorati,
        youtube,
        msdn,
        badoo,
        manu,
        green_black,
        viciao,
        yahoo2,
        weixinShopPager,
        taskPager,
        bootstrap
    }

    public class PagerSocureData
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class PagerBarSettings
    {
        public BarStyle barStyle { get; set; }

        public string Text { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int TotalCount { get; set; }

        public int ShowPageNumber { get; set; }

        public string NoRecordTip { get; set; }

        public string PreWord { get; set; }

        public string NextWord { get; set; }

        public string Onclick { get; set; }

        public string Url { get; set; }

        /// <summary>
        ///   项目中处在iframe中的页码条
        /// </summary>
        public bool InIframe { get; set; }

        public string BarMark { get; set; }

        public string CurrentPageWordFormat { get; set; }

        public bool ShowTotalCount { get; set; }

        public PagerBarSettings() : this(null) { }

        public PagerBarSettings(string url) : this(null, 0, 0, url, false) { }

        public PagerBarSettings(string url, bool inIframe) : this(null, 0, 0, url, inIframe) { }


        public PagerBarSettings(int? pageIndex, int pageCount, int totalCount, string url, bool inIframe)
        {
            this.PageIndex = pageIndex ?? 1;
            this.PageCount = pageCount > 0 ? pageCount : 20;
            this.TotalCount = totalCount;
            this.Url = url;
            this.InIframe = inIframe;

            this.ShowTotalCount = true;
        }
    }


    public static class PagerBarExtension
    {
        public static string PagerBar<T>(this IHtmlHelper html, PagedList<T> dataSource) where T : class
        {
            return PagerBar(html, dataSource, null);
        }

        public static string PagerBar<T>(this IHtmlHelper html, PagedList<T> dataSource, PagerBarSettings settings) where T : class
        {
            settings = settings ?? new PagerBarSettings();
            settings.PageIndex = dataSource.PageIndex;
            settings.PageCount = dataSource.PageCount;
            settings.TotalCount = dataSource.TotalCount;
            return PagerBar(html, settings);
        }


        public static string PagerBar<T>(this IHtmlHelper html, PagerSocureData dataSource, PagerBarSettings settings) where T : class
        {
            settings = settings ?? new PagerBarSettings();
            settings.PageIndex = dataSource.PageIndex;
            settings.PageCount = dataSource.PageCount;
            settings.TotalCount = dataSource.TotalCount;
            return PagerBar(html, settings);
        }



        public static string PagerBar(this IHtmlHelper html, PagerBarSettings settings)
        {
            SetDefaultValue(settings);//设置默认值

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0}\" >", settings.barStyle.ToString());

            //判断空记录
            if (settings.TotalCount == 0)
            {
                sb.Append(settings.NoRecordTip);
                sb.Append("</div>");

                return sb.ToString();
            }

            int totalPage = Convert.ToInt32((settings.TotalCount - 1) / settings.PageCount) + 1;//总页数

            string backPageStyle = (settings.PageIndex <= 1) ? "disabled" : "";
            string nextPageStyle = (settings.PageIndex >= totalPage) ? "disabled" : "";

            var firstDisplayPageEnd = 0;//从第1页显示到xx页
            var bodyDisplayPageStart = 0;//当前页临近最左页码
            var bodyDisplayPageEnd = 0;//当前页临近最右页码
            var endDisplayPageStart = 0;//从第xx页显示到最后一页


            //设定 bodyDisplayPageStart
            bodyDisplayPageStart = (settings.PageIndex - settings.ShowPageNumber <= 1) ? 1 : settings.PageIndex - settings.ShowPageNumber; // (ViewData.pageIndex - ViewData.showPageNumber <= ViewData.showPageNumber) ? ViewData.showPageNumber + 1 : ViewData.pageIndex - ViewData.showPageNumber;


            //设定 bodyDisplayPageEnd
            bodyDisplayPageEnd = (settings.PageIndex + settings.ShowPageNumber >= totalPage) ? totalPage : settings.PageIndex + settings.ShowPageNumber;


            //设定 firstDisplayPageEnd
            if (bodyDisplayPageStart > 1)
            {
                if (bodyDisplayPageStart - settings.ShowPageNumber <= 1)
                {
                    firstDisplayPageEnd = bodyDisplayPageStart - 1;
                }
                else
                {
                    firstDisplayPageEnd = settings.ShowPageNumber;
                }
            }
            else
            {
                firstDisplayPageEnd = 0;
            }

            //设定 endDisplayPageStart
            if (bodyDisplayPageEnd < totalPage)
            {
                if (bodyDisplayPageEnd + settings.ShowPageNumber >= totalPage)
                {
                    endDisplayPageStart = bodyDisplayPageEnd + 1;
                }
                else
                {
                    endDisplayPageStart = totalPage - settings.ShowPageNumber + 1;
                }
            }
            else
            {
                endDisplayPageStart = totalPage + 1;
            }
            //页面参数设定结束


            //开始输出

            // 上一条
            if (settings.PageIndex <= 1)
            {
                sb.Append("<span class=\"").Append(backPageStyle).Append("\">").Append(settings.PreWord).Append("</span>");
            }
            else
            {
                sb.Append(GetPageLink(settings.PageIndex - 1, settings.PageIndex, settings.PreWord, settings.CurrentPageWordFormat, settings.Onclick, settings.Url, settings.BarMark, settings.InIframe));
            }


            //first
            for (var i = 1; i <= firstDisplayPageEnd; i++)
            {
                sb.Append(GetPageLink(i, settings.PageIndex, i.ToString(), settings.CurrentPageWordFormat, settings.Onclick, settings.Url, settings.BarMark, settings.InIframe));
            }

            //省略号
            if (firstDisplayPageEnd + 1 < bodyDisplayPageStart)
            {
                sb.Append("<span>... </span>");
            }

            //body
            for (var i = bodyDisplayPageStart; i <= bodyDisplayPageEnd; i++)
            {
                sb.Append(GetPageLink(i, settings.PageIndex, i.ToString(), settings.CurrentPageWordFormat, settings.Onclick, settings.Url, settings.BarMark, settings.InIframe));
            }

            //省略号
            if (bodyDisplayPageEnd + 1 < endDisplayPageStart)
            {
                sb.Append("<span>... </span>");
            }

            //end
            for (var i = endDisplayPageStart; i <= totalPage; i++)
            {
                sb.Append(GetPageLink(i, settings.PageIndex, i.ToString(), settings.CurrentPageWordFormat, settings.Onclick, settings.Url, settings.BarMark, settings.InIframe));
            }

            // 下一条 
            if (settings.PageIndex >= totalPage)
            {
                sb.Append("<span class=\"").Append(nextPageStyle).Append("\">").Append(settings.NextWord).Append("</span>");
            }
            else
            {
                sb.Append(GetPageLink(settings.PageIndex + 1, settings.PageIndex, settings.NextWord, settings.CurrentPageWordFormat, settings.Onclick, settings.Url, settings.BarMark, settings.InIframe));
            }

            //总数
            if (settings.ShowTotalCount)
            {
                int recordStart = (settings.PageIndex - 1) * settings.PageCount + 1;
                int recordEnd = recordStart + settings.PageCount - 1;
                if (recordEnd > settings.TotalCount)
                {
                    recordEnd = settings.TotalCount;//如果超出最大值，则修正为最大值
                }
                sb.Append("<span class=\"total\" title=\"当前显示第" + (recordStart == recordEnd ? recordStart.ToString() : (recordStart + "-" + recordEnd)) + "条\">总共 <b>" + settings.TotalCount.ToString() + "</b> 条</span>");
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        private static void SetDefaultValue(PagerBarSettings settings)
        {
            if (settings.barStyle == BarStyle.defaultStyle)
            {
                settings.barStyle = BarStyle.quotes;
            }

            if (settings.PageIndex == 0)
            {
                settings.PageIndex = 1;
            }

            if (settings.ShowPageNumber == 0)
            {
                settings.ShowPageNumber = 3;
            }

            if (string.IsNullOrEmpty(settings.PreWord))
            {
                settings.PreWord = "上一页";
            }

            if (string.IsNullOrEmpty(settings.NextWord))
            {
                settings.NextWord = "下一页";
            }

            if (settings.PageCount == 0)
            {
                settings.PageCount = 20;
            }

            if (string.IsNullOrEmpty(settings.CurrentPageWordFormat))
            {
                settings.CurrentPageWordFormat = "{0}";
            }

        }

        private static string GetPageLink(int linkPageIndex, int currentPageIndex, string text, string currentPageWordFormat, string onclick, string url, string barMark, bool inInframe)
        {
            //var pageData = "?page=";//string.Format("{0}page=", (Request.QueryString.Count == 0) ? "?" : "&") + "{0}";//页码参数

            onclick = (onclick != null) ? "onclick=\"" + onclick + "\"" : "";
            onclick = onclick.Replace("{pageindex}", linkPageIndex.ToString());

            var httpContext = SenparcDI.GetService<IHttpContextAccessor>().HttpContext;

            url = (!string.IsNullOrEmpty(url)) ? url.UrlDecode().Replace(currentPageWordFormat, linkPageIndex.ToString()) : "";
            var href = (onclick != null && onclick.IndexOf("return false") != -1) ? "href=\"#" + barMark + "\" " : "href=\"" + url + "\" ";

            var linkHTML = "";

            string formatedText = currentPageWordFormat.IndexOf("{0}") >= 0 ? string.Format(currentPageWordFormat, text) : text;//判断是{0}形式还是自定义替换字符串。

            if (linkPageIndex == currentPageIndex)
            {
                linkHTML = $"<span class=\"current\">{formatedText}</span>";
            }
            else
            {
                linkHTML = "<a " + href + onclick + $" {(inInframe ? "data - iniframe =\"1\"" : null)}>{text}</a>";
            }
            return linkHTML;
        }
    }
}