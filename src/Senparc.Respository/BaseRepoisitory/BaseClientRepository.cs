using Senparc.CO2NET;
using Senparc.Core.Models;
using Senparc.Utility;

namespace Senparc.Repository
{
    public interface IBaseClientRepository<T> : IBaseRepository<T> where T : class, new() // global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        ISqlClientFinanceData DB { get; }
    }

    public class BaseClientRepository<T> : BaseRepository<T>, IBaseClientRepository<T> where T : class, new() // global::System.Data.Objects.DataClasses.EntityObject, new()
    {
        public ISqlClientFinanceData DB
        {
            get
            {
                return base.BaseDB as ISqlClientFinanceData;
            }
        }

        public BaseClientRepository() : this(null) { }
        public BaseClientRepository(ISqlClientFinanceData db)
        {
            //System.Web.HttpContext.Current.Response.Write("-"+this.GetType().Name + "<br />");

            base.BaseDB = db ?? SenparcDI.GetService<ISqlClientFinanceData>();// ObjectFactory.GetInstance<ISqlClientFinanceData>();

            _entitySetName = EntitySetKeys.Keys[typeof(T)];
        }


    }
}
