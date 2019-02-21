using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Senparc.CO2NET;
using Senparc.CO2NET.Extensions;
using Senparc.Scf.Core.Cache;
using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.Core.Models.VD
{
    public interface IPageModelBase : IBaseVD, IValidatorEnvironment
    {
        new RouteData RouteData { get; set; }
    }


    public class PageModelBase : PageModel, IPageModelBase
    {
        public FullSystemConfig FullSystemConfig { get; set; }

        public MetaCollection MetaCollection { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public new RouteData RouteData { get => base.RouteData; set => throw new NotImplementedException(); }

        //public RouteData RouteData { get; set; }
        //另外一种写法：
        //public RouteData GetRouteData()
        //{
        //    return base.RouteData;
        //}

        //public void SetRouteData(RouteData value)
        //{
        //    throw new NotImplementedException();
        //}


        public string CurrentMenu { get; set; }

        public List<Messager> MessagerList { get; set; }

        public FullAccount FullAccount { get; set; }

        public DateTime PageStartTime { get; set; }

        public DateTime PageEndTime { get; set; }

        public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            //获取缓存系统信息
            var fullSystemConfigCache = SenparcDI.GetService<FullSystemConfigCache>();
            FullSystemConfig = fullSystemConfigCache.Data;

            base.OnPageHandlerSelected(context);
        }

        /// <summary>
        /// 检查是否在特定 Scheme 下已登录
        /// </summary>
        /// <param name="authenticationScheme">Scheme 名称</param>
        /// <returns></returns>
        public async Task<bool> CheckLoginedAsync(string authenticationScheme)
        {
            var authenticate = await HttpContext.AuthenticateAsync(authenticationScheme);
            return authenticate.Succeeded;
        }

        public void SetMessager(MessageType messageType, string messageText, bool showClose = true)
        {
            TempData["Messager"] = new Messager(messageType, messageText).ToJson();
        }


    }
}
