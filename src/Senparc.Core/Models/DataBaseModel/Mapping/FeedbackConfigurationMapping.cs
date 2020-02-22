using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;

namespace Senparc.Core.Models
{
    public class FeedbackConfigurationMapping : ConfigurationMappingWithIdBase<FeedBack, int>
    {
        public override void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasQueryFilter(z => !z.Flag);
        }
    }
}