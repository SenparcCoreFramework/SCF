using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Senparc.Core.Models
{
    public class PointsLogConfigurationMapping : IEntityTypeConfiguration<PointsLog>
    {
        public void Configure(EntityTypeBuilder<PointsLog> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(e => e.Points).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(e => e.BeforePoints).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(e => e.AfterPoints).HasColumnType("decimal(18, 2)").IsRequired();
            builder.Property(e => e.AddTime).HasColumnType("datetime").IsRequired();
        }
    }
}
