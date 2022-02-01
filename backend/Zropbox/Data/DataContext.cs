using Zropbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Zropbox.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CDNEntry> CDNEntries { get; set; }
        public DbSet<CDNTempEntry> CDNTEmpEntries { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.Equals("true", Environment.GetEnvironmentVariable("ENABLE_SQL_LOGGING")))
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                })).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CDNEntry>()
                .HasOne(c => c.Parent)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CDNEntry>()
                .HasOne(c => c.UploadedBy)
                .WithMany(c => c.Entries)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CDNTempEntry>()
                .HasOne(c => c.CDNEntry)
                .WithOne(c => c.CDNTempEntry)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
