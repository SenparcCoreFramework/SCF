using Microsoft.EntityFrameworkCore;

namespace Senparc.Scf.Core.Models
{
    public interface ISqlBaseFinanceData
    {
        /// <summary>
        /// 强制手动更改DetectChange
        /// </summary>
        bool ManualDetectChangeObject { get; set; }
        DbContext BaseDataContext { get; }
        void CloseConnection();
    }

    public abstract class SqlBaseFinanceData : ISqlBaseFinanceData
    {
        /// <summary>
        /// 强制手动更改DetectChange
        /// </summary>
        public bool ManualDetectChangeObject { get; set; }
        public abstract DbContext BaseDataContext { get; }

        public abstract void CloseConnection();
    }
}
