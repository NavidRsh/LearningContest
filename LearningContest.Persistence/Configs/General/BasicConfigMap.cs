using CommonInfrastructure.General.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LearningContest.Domain.Entities.General;
using System;

namespace LearningContest.Persistence.Configs.General
{
    public class BasicConfigMap : IEntityTypeConfiguration<BasicConfig>
    {
        public void Configure(EntityTypeBuilder<BasicConfig> builder)
        {
            builder.ToTable("BasicConfig", "dbo");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.ItemKey)
                 .HasConversion(new EnumToStringConverter<BasicConfigKeyEnum>())
                 .HasMaxLength(200);
            builder.Property(e => e.ItemValue).HasMaxLength(200);
            builder.Property(e => e.Description).HasMaxLength(500);

            builder.Property(e => e.Module)
                .HasMaxLength(50)
                .HasConversion(x => x.ToString(), // to converter
                    x => (ModuleEnum)Enum.Parse(typeof(ModuleEnum), x));// from converter


        }
    }
}
