using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;

namespace Senparc.Core.Models
{
    public class PointsLogConfigurationMapping : ConfigurationMappingWithIdBase<PointsLog, int>
    {
        public void Configure(EntityTypeBuilder<PointsLog> builder)
        {
            builder.Property(e => e.Points).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(e => e.BeforePoints).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(e => e.AfterPoints).HasColumnType("decimal(18, 2)").IsRequired();
        }
    }
}
