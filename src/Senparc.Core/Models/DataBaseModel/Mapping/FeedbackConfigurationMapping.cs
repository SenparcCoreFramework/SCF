using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Senparc.Core.Models
{
    public class FeedbackConfigurationMapping : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasKey(z => z.Id);

            builder.Property(e => e.AddTime)
                .HasColumnType("datetime")
                .IsRequired();

            builder.HasQueryFilter(z => !z.Flag);
        }
    }
}