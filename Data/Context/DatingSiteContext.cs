using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Enitities;

namespace Data.Context
{
    public class DatingSiteContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Timers> Timers { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<CallRecord> CallRecords { get; set; }

        public DatingSiteContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CallRecord>()
        .HasOne(cr => cr.User)
        .WithMany(u => u.CallRecords)
        .HasForeignKey(cr => cr.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CallRecord>()
                 .HasOne(cr => cr.Camgirl)
                 .WithMany()
                 .HasForeignKey(cr => cr.CamgirlId)
                 .OnDelete(DeleteBehavior.Restrict);
            SeedRoles(builder);
        }

        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "ADMIN", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "USER", ConcurrencyStamp = "2", NormalizedName = "USER" },
            new IdentityRole() { Name = "CAMGIRL", ConcurrencyStamp = "3", NormalizedName = "CAMGIRL" });
        }
    }
}