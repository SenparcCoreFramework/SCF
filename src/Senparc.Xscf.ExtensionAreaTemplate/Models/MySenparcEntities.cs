using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel
{
    public class MySenparcEntities : XscfDatabaseDbContext
    {
        public override IXscfDatabase XscfDatabaseRegister => new Register();
        public MySenparcEntities(DbContextOptions<MySenparcEntities> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Color> Colors { get; set; }


        //如无特殊需需要，OnModelCreating 方法可以不用写，已经在 Register 中要求注册
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}
