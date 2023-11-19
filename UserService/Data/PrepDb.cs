using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public static class PrepDb
    {

        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {

            // Update the database schema based on the latest changes defined in the application's code.
            if (isProd)
            {
                try
                {
                    Console.WriteLine("--> Attempting to apply migrations...");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            // Add some data if the database is empty
            if (!context.Kiteschools.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.Kiteschools.AddRange(
                    new Kiteschool() { Name = "BLOW Kitesurfschool", Location = "Zandmotor", Email = "contact@blow.com" },
                    new Kiteschool() { Name = "BeachBreak", Location = "Noordwijk", Email = "info@beachbreak.nl" },
                    new Kiteschool() { Name = "Kitesurfspot", Location = "The Hague", Email = "info@kitesurfspot.nl" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}