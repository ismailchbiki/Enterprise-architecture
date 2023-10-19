using CommandsService.Data;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

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