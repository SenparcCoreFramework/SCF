using Senparc.Repository;

namespace Senparc.Scf.Service
{
    public interface IBaseServiceData
    {
        IBaseData BaseData { get; set; }
        void CloseConnection();
    }


    public class BaseServiceData : IBaseServiceData
    {
        public IBaseData BaseData { get; set; }

        public BaseServiceData(IBaseData baseData)
        {
            BaseData = baseData;
        }

        public virtual void CloseConnection()
        {
            BaseData.CloseConnection();
        }
    }
}