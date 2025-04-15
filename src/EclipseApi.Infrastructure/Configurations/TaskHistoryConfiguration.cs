using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EclipseApi.Domain.Entities;


namespace EclipseApi.Infrastructure.Configurations
{
    public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
    {
        public void Configure(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.ToTable("TaskHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.ChangedField)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.PreviousValue)
                .HasMaxLength(500);

            builder.Property(h => h.NewValue)
                .HasMaxLength(500);

            builder.Property(h => h.ChangedDate)
                .IsRequired();

            builder.Property(h => h.ChangedBy)
                .IsRequired();

            builder.HasOne(h => h.Task)
                .WithMany(t => t.Histories)
                .HasForeignKey(h => h.TaskId);
        }
    }
}