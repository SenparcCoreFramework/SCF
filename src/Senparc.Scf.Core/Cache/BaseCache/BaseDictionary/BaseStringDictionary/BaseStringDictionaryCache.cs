using Senparc.Scf.Core.Models;

namespace Senparc.Scf.Core.Cache
{
    public interface IBaseStringDictionaryCache<TValue> : IBaseStringDictionaryCache<TValue, TValue> where TValue : class,new()
    {
    }
    public interface IBaseStringDictionaryCache<TValue, TEntity> : IBaseDictionaryCache<string, TValue, TEntity> where TValue : class,new()
    {

    }

    public abstract class BaseStringDictionaryCache<TValue> : BaseStringDictionaryCache<TValue, TValue>,
                                                                       IBaseStringDictionaryCache<TValue>
        where TValue : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CACHE_KEY"></param>
        /// <param name="db"></param>
        /// <param name="timeOut">单位：分钟。1440为一天。</param>
        public BaseStringDictionaryCache(string CACHE_KEY, ISqlBaseFinanceData db, int timeOut)
            : base(CACHE_KEY, db, timeOut)
        {
            base.TimeOut = timeOut;
        }
    }

    public abstract class BaseStringDictionaryCache<TValue, TEntity> : BaseDictionaryCache<string, TValue, TEntity>, IBaseStringDictionaryCache<TValue, TEntity> 
        where TValue : class,new()
        where TEntity : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CACHE_KEY"></param>
        /// <param name="db"></param>
        /// <param name="timeOut">单位：分钟。1440为一天。</param>
        public BaseStringDictionaryCache(string CACHE_KEY, ISqlBaseFinanceData db, int timeOut)
            : base(CACHE_KEY, db, timeOut)
        {
            base.TimeOut = timeOut;
        }
    }
}
