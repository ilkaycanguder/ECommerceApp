using ECommerceApp.Log.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApp.Log.Infrastructure.Persistence.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Level)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Source)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Exception)
                .HasMaxLength(4000);

            builder.HasIndex(x => x.Level);
            builder.HasIndex(x => x.Source);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}
