using Senparc.Scf.Log;
using System;

namespace Senparc.Scf.Core.Utility
{
    public static class FullEntityCache
    {
        public static void SetFullEntityCache<TFullEntity, TEntity>(TFullEntity fullEntity, TEntity entity)
        {
            if (fullEntity == null || entity == null)
            {
                throw new Exception("参数不可以为NULL");
            }

            try
            {
                var fullEntityType = fullEntity.GetType();
                var entityType = entity.GetType();
                var props = fullEntityType.GetProperties();
                foreach (var p in props)
                {
                    //获得当前属性的特性
                    AutoSetCacheAttribute m = Attribute.GetCustomAttribute(p, typeof(AutoSetCacheAttribute)) as AutoSetCacheAttribute;
                    if (m != null)
                    {
                        //允许自动复制
                        //获取原始实体值
                        var entityProp = entityType.GetProperty(p.Name);
                        if (entityProp == null || entityProp.PropertyType != p.PropertyType)
                        {
                            throw new Exception("原始实体没有相同类型和名称的属性存在：" + p.Name);
                        }
                        p.SetValue(fullEntity, entityProp.GetValue(entity, null), null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.SystemLogger.Debug("跟踪Cache错误：" + ex.Message, ex);
                throw ex;
            }
        }
    }
}