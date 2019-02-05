using Senparc.Scf.Core.Models;

namespace Senparc.Repository
{
    public interface IBaseData
    {
        ISqlBaseFinanceData BaseDB { get; set; }

        void CloseConnection();
    }


    public class BaseData : IBaseData
    {
        public ISqlBaseFinanceData BaseDB { get; set; }

        public BaseData(ISqlBaseFinanceData baseDB)
        {
            BaseDB = baseDB;
        }

        public virtual void CloseConnection()
        {
            BaseDB.CloseConnection();
        }
    }
}