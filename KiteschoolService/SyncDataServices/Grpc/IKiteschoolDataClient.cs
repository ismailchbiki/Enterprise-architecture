using KiteschoolService.Models;

namespace KiteschoolService.SyncDataServices.Grpc
{
    public interface IKiteschoolDataClient
    {
        IEnumerable<Kiteschool> ReturnAllKiteschools();
    }
}