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
        // public DbSet<Command> Commands { get; set; }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the relationship between Kiteschool and Command
            // modelBuilder.Entity<Kiteschool>()
            //     .WithOne(c => c.Platform!)
            //     .HasForeignKey(c => c.PlatformId);
            // // .HasMany(p => p.Commands)

            // // Seed data
            // modelBuilder.Entity<Command>()
            //     .HasOne(c => c.Platform)
            //     .WithMany(p => p.Commands)
            //     .HasForeignKey(c => c.PlatformId);
        }
    }
}