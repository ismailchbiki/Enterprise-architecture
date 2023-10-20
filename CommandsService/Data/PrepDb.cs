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

            foreach (var plat in platforms)
            {
                if (!commandRepo.ExternalPlatformExists(plat.ExternalID))
                {
                    commandRepo.CreatePlatform(plat);
                }

                commandRepo.SaveChanges();
            }
        }
    }
}