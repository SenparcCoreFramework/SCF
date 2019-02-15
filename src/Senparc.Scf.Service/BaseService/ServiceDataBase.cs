using Senparc.Scf.Repository;

namespace Senparc.Scf.Service
{
    public interface IServiceDataBase
    {
        IDataBase BaseData { get; set; }
        void CloseConnection();
    }


    public class ServiceDataBase : IServiceDataBase
    {
        public IDataBase BaseData { get; set; }

        public ServiceDataBase(IDataBase baseData)
        {
            BaseData = baseData;
        }

        public virtual void CloseConnection()
        {
            BaseData.CloseConnection();
        }
    }
}