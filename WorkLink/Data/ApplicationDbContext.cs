using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkLink.Models;

namespace WorkLink.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 🔹 DbSets
        public DbSet<WorkerProfile> WorkerProfiles { get; set; }
        public DbSet<WorkerAvailability> WorkerAvailabilities { get; set; }
        public DbSet<WorkerVerification> WorkerVerifications { get; set; } = null!;

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔹 User ↔ WorkerProfile (1–1)
            builder.Entity<WorkerProfile>()
                .HasOne(w => w.User)
                .WithOne(u => u.WorkerProfile)
                .HasForeignKey<WorkerProfile>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 User ↔ Jobs (1–Many)
            builder.Entity<Job>()
                .HasOne(j => j.User)
                .WithMany(u => u.Jobs)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 WorkerProfile ↔ Jobs (1–Many)
            builder.Entity<Job>()
                .HasOne(j => j.WorkerProfile)
                .WithMany(w => w.Jobs)
                .HasForeignKey(j => j.WorkerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
