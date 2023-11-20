using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IKiteschoolDataClient
    {
        Task SendKiteschoolToKiteschoolService(KiteschoolReadDto kiteschool);
    }
}