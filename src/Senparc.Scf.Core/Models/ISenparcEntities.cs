using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.Core.Models
{
    public interface ISenparcEntities : IDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbQueryCache, IDbContextPoolable
    {
        void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase;
    }
}
