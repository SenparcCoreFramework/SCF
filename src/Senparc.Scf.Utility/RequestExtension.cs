using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace Senparc.Scf.Utility
{
    public static class RequestExtension
    {
        private const string NullIpAddress = "::1";

        //public static bool IsLocal(this HttpRequest req)
        //{
        //    var connection = req.HttpContext.Connection;
        //    if (connection.RemoteIpAddress.IsSet())
        //    {
        //        //We have a remote address set up
        //        return connection.LocalIpAddress.IsSet()
        //            //Is local is same as remote, then we are local
        //            ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
        //            //else we are remote if the remote IP address is not a loopback address
        //            : IPAddress.IsLoopback(connection.RemoteIpAddress);
        //    }

        //    return true;
        //}

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }


        ///// <summary>
        ///// Determines whether the specified HTTP request is an AJAX request.
        ///// </summary>
        ///// 
        ///// <returns>
        ///// true if the specified HTTP request is an AJAX request; otherwise, false.
        ///// </returns>
        ///// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        //public static bool IsAjaxRequest(this HttpRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException("request");

        //    if (request.Headers != null)
        //        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        //    return false;
        //}

        ///// <summary>
        ///// 通常是以/开头的完整路径
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public static string PathAndQuery(this HttpRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException("request");

        //    return request.Path + request.QueryString;
        //}

        ///// <summary>
        ///// 获取来源页面
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public static string UrlReferrer(this HttpRequest request)
        //{
        //    return request.Headers["Referer"].ToString();
        //}

        /// <summary>
        /// 返回绝对地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string AbsoluteUri(this HttpRequest request)
        {
            var absoluteUri = string.Concat(
                          request.Scheme,
                          "://",
                          request.Host.ToUriComponent(),
                          request.PathBase.ToUriComponent(),
                          request.Path.ToUriComponent(),
                          request.QueryString.ToUriComponent());

            return absoluteUri;
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }


        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static AgentType UserAgentType(this HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString();
            switch (userAgent)
            {
                case string android when android.Contains("MicroMessenger"):
                    return AgentType.Wechat;
                case string android when android.Contains("Android"):
                    return AgentType.Android;
                case string android when android.Contains("iPhone"):
                    return AgentType.IPhone;
                case string android when android.Contains("iPad"):
                    return AgentType.IPad;
                case string android when android.Contains("Windows Phone"):
                    return AgentType.WindowsPhone;
                case string android when android.Contains("Windows NT"):
                    return AgentType.Windows;
                case string android when android.Contains("Mac OS"):
                    return AgentType.MacOS;
            }
            return AgentType.Android;
        }

        public enum AgentType
        {
            Android = 0,
            IPhone = 1,
            IPad = 2,
            WindowsPhone = 3,
            Windows = 4,
            Wechat = 6,
            MacOS = 7

        }


        /// <summary>
        /// 获取客户端地址（IP）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IPAddress UserHostAddress(this HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        }
    }

}
