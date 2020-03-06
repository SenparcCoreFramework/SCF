using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Senparc.ExtensionAreaTemplate.Models.DatabaseModel
{
    /// <summary>
    /// IDbContextProvider
    /// </summary>
    public interface IDbContextProvider
    {
        SenparcEntitiesBase Get();
    }


    public class DbContextProvider : IDbContextProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SenparcEntitiesBase Get()
        {
            var senparcEntities =  _serviceProvider.GetService<SenparcEntitiesBase>();
            //senparcEntities.
                return senparcEntities;
        }
    }
}
