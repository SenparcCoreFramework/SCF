using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    //参考：https://www.cnblogs.com/yuangang/archive/2016/08/08/5743660.html

    public static class Extensions
    {

        public static IServiceCollection AddSenparcMvcDI(this IServiceCollection services)
        {
            DI.ServiceCollection = services;
            DI.ServiceProvider = services.BuildServiceProvider();
            return services;
        }

        public static IApplicationBuilder UseSenparcMvcDI(this IApplicationBuilder builder)
        {
            DI.ServiceProvider = builder.ApplicationServices;
            return builder;
        }
    }

    /// <summary>
    /// TODO:和SenparcDI合并
    /// </summary>
    public static class DI
    {
        public static IServiceCollection ServiceCollection { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
