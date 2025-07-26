
//using HomeZone.Services.Wallet.Model;

using HomeZone.Services.SavingsAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace HomeZone.Services.SavingsAPI.Data
{

    public class AppDbContext : DbContext
    {
        // Constructor to accept DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<SavingsPlan> SavingsPlans { get; set; }
        public DbSet<SavingsSchedule> SavingsSchedules { get; set; }

        // Configure model relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SavingsPlan entity
            modelBuilder.Entity<SavingsPlan>(entity =>
            {
                entity.HasKey(p => p.Id); // Primary Key

                entity.Property(p => p.PlanName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Purpose)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(p => p.AmountPerPeriod)
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.TotalAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Frequency)
                      .IsRequired()
                      .HasMaxLength(50);

                // One-to-many: A plan has many schedules
                entity.HasMany(p => p.Schedules)
                      .WithOne(s => s.SavingsPlan)
                      .HasForeignKey(s => s.SavingsPlanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SavingsSchedule entity
            modelBuilder.Entity<SavingsSchedule>( entity =>
            {
                entity.HasKey(s => s.Id); // Primary Key

                entity.Property(s => s.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(s => s.ScheduledDate)
                      .IsRequired();
            });
        }
    }



}
