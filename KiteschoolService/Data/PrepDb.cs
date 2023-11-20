using KiteschoolService.Data;
using KiteschoolService.Models;
using KiteschoolService.SyncDataServices.Grpc;

namespace KiteschoolService.Data
{
    public static class PrepDb
    {
        // This will call gRPC server to get latest updates on kiteschools data
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IKiteschoolDataClient>();

                var kiteschools = grpcClient.ReturnAllKiteschools();

                // After getting all kiteschools from gRPC server, seed the data into the KiteschoolsService database.
                SeedData(serviceScope.ServiceProvider.GetService<IKiteschoolRepo>(), kiteschools);
            }
        }

        private static void SeedData(IKiteschoolRepo kiteschoolRepo, IEnumerable<Kiteschool> kiteschools)
        {
            Console.WriteLine("--> Seeding new kiteschools...");

            if (kiteschools == null || !kiteschools.Any())
            {
                Console.WriteLine("No kiteschools to seed.");
                return;
            }

            foreach (var kiteschool in kiteschools)
            {
                try
                {
                    if (!kiteschoolRepo.ExternalKiteschoolExists(kiteschool.ExternalID))
                    {
                        kiteschoolRepo.CreateKiteschool(kiteschool);
                        Console.WriteLine($"Kiteschool with ExternalID {kiteschool.ExternalID} seeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Kiteschool with ExternalID {kiteschool.ExternalID} already exists, skipping.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding kiteschool with ExternalID {kiteschool.ExternalID}: {ex.Message}");
                }
            }

            kiteschoolRepo.SaveChanges();
        }
    }
}