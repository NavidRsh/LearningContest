using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearningContest.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Persistence.Configs
{
    public abstract class RecordHistoryConfig<T> where T : RecordHistory
    {
        public void ConfigHistory(EntityTypeBuilder<T> builder, string createDateTimeDbType = "", string modifyDateTimeDbType = "")
        {

            builder.Property(e => e.CreateDateTime).IsRequired();
            if (!string.IsNullOrEmpty(createDateTimeDbType))
            {
                builder.Property(e => e.CreateDateTime).HasColumnType(createDateTimeDbType);
            }
            if (!string.IsNullOrEmpty(modifyDateTimeDbType))
            {
                builder.Property(e => e.ModifyDateTime).HasColumnType(modifyDateTimeDbType);
            }

            //builder.HasOne(e => e.CreatedBy)
            //  .WithMany().HasForeignKey(e => e.CreatedById)
            //  .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(e => e.ModifiedBy)
            //    .WithMany().HasForeignKey(e => e.ModifiedById)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
