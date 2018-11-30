namespace Senparc.Core.Models
{
    public interface IBaseModel : ISoftDelete
    {

    }
    public class BaseModel : IBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Flag { get; set; }
    }
}
