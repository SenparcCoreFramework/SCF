using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.XscfBase.Attributes;

namespace Senparc.Core.Models
{
    [XscfAutoConfigurationMapping]
    public class FeedbackConfigurationMapping : ConfigurationMappingWithIdBase<FeedBack, int>
    {
        public override void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasQueryFilter(z => !z.Flag);
        }
    }
}