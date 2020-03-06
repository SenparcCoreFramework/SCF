using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Scf.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.ExtensionAreaTemplate.Models
{
    public class AreaTemplate_ColorConfigurationMapping : ConfigurationMappingWithIdBase<AreaTemplate_Color, int>
    {
        public override void Configure(EntityTypeBuilder<AreaTemplate_Color> builder)
        {
            builder.Property(e => e.Red).IsRequired();
            builder.Property(e => e.Green).IsRequired();
            builder.Property(e => e.Bule).IsRequired();
        }
    }
}
