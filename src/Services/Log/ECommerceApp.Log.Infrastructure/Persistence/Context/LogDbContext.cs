using ECommerceApp.Log.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Log.Infrastructure.Persistence.Context
{
    public class LogDbContext : DbContext
    {
        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

        public DbSet<LogEntry> LogEntries => Set<LogEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
