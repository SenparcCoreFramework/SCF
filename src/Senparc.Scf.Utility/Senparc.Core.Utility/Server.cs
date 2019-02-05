using Microsoft.AspNetCore.Http;

namespace Senparc.Scf.Core.Utility
{
    public static class Server
    {
        private static string _appDomainAppPath;

        public static string AppDomainAppPath
        {
            get
            {
                if (_appDomainAppPath == null)
                {
                    _appDomainAppPath = SenparcHttpContext.ContentRootPath;
                }
                return _appDomainAppPath;
            }
            set
            {
                _appDomainAppPath = value;
            }
        }

        public static string GetMapPath(string virtualPath)
        {
            if (virtualPath == null)
            {
                return "";
            }

            return SenparcHttpContext.MapPath(virtualPath);
        }

        public static string GetWebMapPath(string virtualPath)
        {
            if (virtualPath == null)
            {
                return "";
            }

            return SenparcHttpContext.MapWebPath(virtualPath);
        }

        public static HttpContext HttpContext
        {
            get
            {
                return SenparcHttpContext.Current;
            }
        }
    }
}