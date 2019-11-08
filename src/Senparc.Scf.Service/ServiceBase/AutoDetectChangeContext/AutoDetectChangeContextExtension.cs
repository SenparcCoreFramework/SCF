using Microsoft.EntityFrameworkCore;

namespace Senparc.Scf.Service
{
    public static class AutoDetectChangeContextExtension
    {
       /// <summary>
        /// 创建AutoDetectChangeContext的实例
       /// </summary>
       /// <param name="serviceData"></param>
       /// <returns></returns>
       public static AutoDetectChangeContextWrap InstanceAutoDetectChangeContextWrap(this IServiceDataBase serviceData)
       {
           return new AutoDetectChangeContextWrap(serviceData);
       }

       /// <summary>
       /// 创建CloseAutoDetectChangeContext的实例
       /// </summary>
       /// <param name="wrap">AutoDetectChangeContextWrap实例</param>
       /// <returns></returns>
       public static CloseAutoDetectChangeContext InstanceCloseAutoDetectChangeContext(this AutoDetectChangeContextWrap wrap)
       {
           return new CloseAutoDetectChangeContext(wrap);
       }

       /// <summary>
       /// 创建CloseAutoDetectChangeContext的实例
       /// </summary>
       /// <param name="wrap">AutoDetectChangeContextWrap实例</param>
       /// <returns></wrap>
       public static void ForceDetectChange(this AutoDetectChangeContextWrap wrap, object entity)
       {
           wrap.ServiceData.BaseData.BaseDB.BaseDataContext.Entry(entity).State = EntityState.Modified;
       }
    }
}
