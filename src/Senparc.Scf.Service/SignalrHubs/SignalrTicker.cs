using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Scf.Service.SignalrHubs
{
    /// <summary>
    /// 记录 SignalR 全局上下文的类，可轮询执行操作
    /// </summary>
    public class SignalrTicker<THub>
        where THub : Hub
    {
        private static /*readonly*/ SignalrTicker<THub> _instance;

        private readonly IHubContext<THub> m_context;

        public IHubContext<THub> GlobalContext
        {
            get { return m_context; }
        }

        /// <summary>
        /// 静态实例对象
        /// </summary>
        public static SignalrTicker<THub> Instance(HttpContext httpContext)
        {
            if (_instance == null)
            {
                if (httpContext!=null)
                {
                    var hubContext = httpContext.RequestServices.GetRequiredService<IHubContext<THub>>();
                    _instance = new SignalrTicker<THub>(hubContext);
                }
            }
            return _instance;
        }

        /// <summary>
        /// 轮询间隔时间（毫秒），默认值：500
        /// </summary>
        public int SleepMillionSeconds { get; set; } = 500;

        /// <summary>
        /// 参数：SignalrTicker<THub>，当前轮询的次数
        /// </summary>
        public Action<SignalrTicker<THub>, int> SenderAction { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        private SignalrTicker(IHubContext<THub> context)
        {
            m_context = context;
            //这里不能直接调用Sender，因为Sender是一个不退出的“死循环”，否则这个构造函数将不会退出。  
            //其他的流程也将不会再执行下去了。所以要采用异步的方式。  
            Task.Run(() => Sender());
        }

        /// <summary>
        /// 注册 SignalrTicker，激活静态变量
        /// </summary>
        public static void Register()
        {
            //可以写一些初始化
        }

        public void Sender()
        {
            int count = 0;
            while (true)
            {
                SenderAction?.Invoke(this, count);
                count++;
                Thread.Sleep(SleepMillionSeconds);
            }
        }
    }
}
