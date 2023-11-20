using CommandsService.Data;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        // This will call gRPC server to get latest updates on platforms data
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                // After getting all platforms from gRPC server, seed the data into the CommandService database.
                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo commandRepo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new Platforms...");

            if (platforms == null || !platforms.Any())
            {
                Console.WriteLine("No platforms to seed.");
                return;
            }

            foreach (var plat in platforms)
            {
                try
                {
                    if (!commandRepo.ExternalPlatformExists(plat.ExternalID))
                    {
                        commandRepo.CreatePlatform(plat);
                        Console.WriteLine($"Platform with ExternalID {plat.ExternalID} seeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Platform with ExternalID {plat.ExternalID} already exists, skipping.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding platform with ExternalID {plat.ExternalID}: {ex.Message}");
                }
            }

            commandRepo.SaveChanges();
        }
    }
}