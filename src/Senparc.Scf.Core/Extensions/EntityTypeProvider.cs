using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Senparc.Scf.Core.Models
{
    public interface IEntityTypeProvider
    {
        IEnumerable<Type> GetEntityTypes(Type type);
    }

    public class DefaultEntityTypeProvider : IEntityTypeProvider
    {
        private static IList<Type> _entityTypeCache;
        public IEnumerable<Type> GetEntityTypes(Type type)
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }

            _entityTypeCache = (from assembly in GetReferencingAssemblies()
                                from definedType in assembly.DefinedTypes
                                where definedType.BaseType == type
                                select definedType.AsType()).ToList();
            return _entityTypeCache;
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            return from library in dependencies
                   select Assembly.Load(new AssemblyName(library.Name));
        }
    }
}