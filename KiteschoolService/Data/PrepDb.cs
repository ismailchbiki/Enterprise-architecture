using KiteschoolService.Models;

namespace KiteschoolService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<IKiteschoolRepo>());
            }
        }

        private static void SeedData(IKiteschoolRepo kiteschoolRepo)
        {
            Console.WriteLine("--> Seeding new kiteschools...");

            // Check if the collection is empty
            if (!kiteschoolRepo.GetAllKiteschools().Any())
            {
                // Add sample data
                var kiteschools = new List<Kiteschool>
            {
                new Kiteschool
                {
                    Name = "Kite Academy",
                    Location = "Beach City",
                    Email = "info@kiteacademy.com",
                    CreatedByUserId = 1
                },
                new Kiteschool
                {
                    Name = "Wind Riders",
                    Location = "Coastal Town",
                    Email = "contact@windriders.com",
                    CreatedByUserId = 2
                },
            };

                kiteschoolRepo.CreateManyKiteschools(kiteschools);
                Console.WriteLine($"Seeded {kiteschools.Count} kiteschools.");
            }
            else
            {
                Console.WriteLine("Database already contains kiteschool data. No seeding required.");
            }
        }
    }
}