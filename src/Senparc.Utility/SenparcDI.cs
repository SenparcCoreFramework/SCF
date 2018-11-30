using Microsoft.Extensions.DependencyInjection;

namespace Senparc.Utility
{
    public static class SenparcDI
    {
        public static IServiceCollection ServiceCollection { get; private set; }

        /// <summary>
        /// 注册并保留一个全局的ServiceCollection变量
        /// </summary>
        /// <param name="ServiceCollection"></param>
        public static IServiceCollection AddSenparcDI(this IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            return serviceCollection;
        }

        /// <summary>
        /// 创建一个新的 ServiceCollection 对象
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection GetServiceCollection()
        {
            return ServiceCollection;
        }

        /// <summary>
        /// 获取 ServiceProvider
        /// </summary>
        /// <returns></returns>
        public static ServiceProvider GetServiceProvider()
        {
            return GetServiceCollection().BuildServiceProvider();
        }

        /// <summary>
        /// 使用 .net core 默认的 DI 方法获得实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return GetServiceProvider().GetService<T>();
        }
    }
}
