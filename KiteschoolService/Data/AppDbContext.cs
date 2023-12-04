using KiteschoolService.Models;
using Microsoft.EntityFrameworkCore;

namespace KiteschoolService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        // DbSet is a collection of entities
        public DbSet<Kiteschool> Kiteschools { get; set; }
        // public DbSet<UserInteraction> UserInteractions { get; set; }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the relationship between Kiteschool and UserInteraction
            // modelBuilder.Entity<Kiteschool>()
            //     .WithOne(c => c.Kiteschool!)
            //     .HasForeignKey(c => c.KiteschoolId);
            // // .HasMany(p => p.Kiteschools)

            // // Seed data
            // modelBuilder.Entity<UserInteraction>()
            //     .HasOne(c => c.Kiteschool)
            //     .WithMany(p => p.Kiteschools)
            //     .HasForeignKey(c => c.KiteschoolId);
        }
    }
}