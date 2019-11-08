using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Microsoft.AspNetCore.Http
{
    //参考：https://www.cnblogs.com/yuangang/archive/2016/08/08/5743660.html

    public static class SenparcHttpContext
    {
        /// <summary>
        /// 目录分隔符：Windows下“\”，Mac OS和Linux下“\”
        /// </summary>
        public static string DirectorySeparatorChar { get; } = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// 包含引用程序的目录绝对路径
        /// </summary>
        public static string ContentRootPath { get; } = DI.ServiceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath;

        /// <summary>
        /// 包含引用程序的目录绝对路径
        /// </summary>
        public static string ContentWebRootPath { get; } = DI.ServiceProvider.GetRequiredService<IHostingEnvironment>().WebRootPath;


        public static HttpContext Current
        {
            get
            {
                try
                {
                    object factory = DI.ServiceProvider.GetService(typeof(IHttpContextAccessor));
                    HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                    return context;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return IsAbsolute(path)
                ? path
                : Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string MapWebPath(string path)
        {
            return IsAbsolute(path)
                ? path
                : Path.Combine(ContentWebRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }

        /// <summary>
        /// 是否是绝对路径
        /// windows下判断 路径是否包含 ":"
        /// Mac OS、Linux下判断 路径是否包含 "\"
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsAbsolute(string path)
        {
            return Path.VolumeSeparatorChar == ':' ? path.IndexOf(Path.VolumeSeparatorChar) > 0 : path.IndexOf('\\') > 0;
        }
    }
}
