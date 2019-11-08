namespace Senparc.Scf.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 判断对象是否为null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(object obj)
        {
            return obj == null;
        }
    }
}