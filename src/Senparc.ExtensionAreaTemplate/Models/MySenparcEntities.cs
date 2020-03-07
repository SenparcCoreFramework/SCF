using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Models.DatabaseModel
{
    public class MySenparcEntities : DbContext
    {
        public MySenparcEntities(DbContextOptions<MySenparcEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaTemplate_ColorConfigurationMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
