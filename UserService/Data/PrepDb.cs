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

            SeedUsers(context);
        }

        private static void SeedUsers(AppDbContext context)
        {
            // Add some data if the Users table is empty
            if (!context.Users.Any())
            {
                Console.WriteLine("--> Seeding User data...");

                context.Users.AddRange(
                    new User()
                    {
                        Firstname = "Ismail",
                        Lastname = "Chbiki",
                        Email = "ismail.chbiki@gmail.com",
                        Password = "hashed_password",
                        Role = "Admin"
                    },
                    new User()
                    {
                        Firstname = "Jane",
                        Lastname = "Doe",
                        Email = "jane.doe@gmail.com",
                        Password = "hashed_password",
                        Role = "User"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> User data already exists");
            }
        }
    }
}