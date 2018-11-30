namespace Senparc.Core.Models
{
    /// <summary>
    /// 软删除接口
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool Flag { get; set; }
    }
}
