using System;
using System.Collections.Generic;
using System.Linq;
using Senparc.CO2NET;
using Senparc.Scf.Core.DI;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Utility;

namespace Senparc.Scf.Core.Cache
{
    public interface ISendEmailCache : IBaseCache<List<AutoSendEmail>>
    {
        List<AutoSendEmail> GetAllList();
        bool InsertEmail(AutoSendEmail email);
        bool SendSuccess(AutoSendEmail email);
        bool SendFail(AutoSendEmail email);
        bool SetSendCount(List<int> idList, int sendCount);
        void DeleteEmail(List<int> ids);
    }

    [AutoDIType(DILifecycleType.Singleton)]
    public class SendEmailCache : BaseCache<List<AutoSendEmail>>, ISendEmailCache
    {
        public const string CACHE_KEY = "SendEmailCache";

        //private string filePath;
        private XmlDataContext xmlCtx;
        //private HttpContext _context;
        public SendEmailCache(/*HttpContext context*/)
            : base(CACHE_KEY, null)
        {
            base.TimeOut = 1440;
            xmlCtx = new XmlDataContext();//TODO:注入
            //filePath = ServerUtility.ContentRootMapPath("~/APP_Data/SendEmail.config");//context.Server.MapPath("~/APP_Data/SendEmail.config");
            //HttpContext.Current.Response.Write(filePath);
        }


        public override List<AutoSendEmail> Update()
        {
            int maxSendCount = Config.SiteConfig.MaxSendEmailTimes;//最大重复发送次数
            List<AutoSendEmail> data = this.GetAllList()
                                        .Where(z => z.SendCount < maxSendCount).OrderBy(z => z.SendCount).OrderBy(z => z.Id).ToList();
            base.SetData(data, base.TimeOut, null);
            return data;
        }

        public List<AutoSendEmail> GetAllList()
        {
            return xmlCtx.GetXmlList<AutoSendEmail>();
        }

        public bool InsertEmail(AutoSendEmail email)
        {
            bool success = xmlCtx.Insert(email);
            if (success)
            {
                this.Update();
            }
            return success;
        }

        public void DeleteEmail(List<int> ids)
        {
            var allList = this.GetAllList();
            foreach (var id in ids)
            {
                var email = allList.FirstOrDefault(z => z.Id == id);
                if (email == null)
                {
                    continue;
                }
                xmlCtx.Delete(email);
            }
            this.Update();
        }

        public bool SendSuccess(AutoSendEmail email)
        {
            bool success = xmlCtx.Delete(email);
            if (success)
            {
                success = this.Data.Remove(email);
            }
            return success;
        }

        public bool SendFail(AutoSendEmail email)
        {
            email.SendCount++;
            email.LastSendTime = DateTime.Now;
            bool success = xmlCtx.Save(new[] { email });
            if (success)
            {
                this.Update();
            }
            return success;
        }

        public bool SetSendCount(List<int> idList, int sendCount)
        {
            var changeList = this.GetAllList().FindAll(z => idList.Contains(z.Id));
            changeList.ForEach(z => z.SendCount = sendCount);
            bool success = xmlCtx.Save(changeList);
            if (success)
            {
                this.Update();
            }
            return success;
        }
    }
}
