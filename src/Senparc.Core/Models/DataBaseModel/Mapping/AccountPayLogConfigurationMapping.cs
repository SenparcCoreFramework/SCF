using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Senparc.Core.Models
{
    public class AccountPayLogConfigurationMapping : IEntityTypeConfiguration<AccountPayLog>
    {
        public void Configure(EntityTypeBuilder<AccountPayLog> builder)
        {
            builder.HasKey(z => z.Id);
            builder.Property(e => e.OrderNumber).HasColumnType("varchar(100)").IsRequired();
            builder.Property(e => e.TotalPrice).HasColumnType("money").IsRequired();
            builder.Property(e => e.Price).HasColumnType("money").IsRequired();
            builder.Property(e => e.Fee).HasColumnType("money").IsRequired();
            builder.Property(e => e.PayMoney).HasColumnType("money").IsRequired();
            builder.Property(e => e.UsedPoints).HasColumnType("decimal(18, 2)").IsRequired(false);
            builder.Property(e => e.Fee).HasColumnType("money").IsRequired();
            builder.Property(e => e.Fee).HasColumnType("money").IsRequired();
            builder.Property(e => e.GetPoints).HasColumnType("money").IsRequired();
            builder.Property(e => e.AddIp).HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.CompleteTime).HasColumnType("datetime").IsRequired();
            builder.Property(e => e.AddTime).HasColumnType("datetime").IsRequired();
            builder.Property(e => e.Description).HasColumnType("varchar(250)").IsRequired();
            builder.Property(e => e.TradeNumber).HasColumnType("varchar(150)").IsRequired(false);
            builder.Property(e => e.PrepayId).HasColumnType("varchar(100)").IsRequired(false);

            builder.HasMany(z => z.PointsLogs).WithOne(z => z.AccountPayLog).HasForeignKey(z => z.AccountPayLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
