using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IKiteschoolDataClient
    {

        Task<IEnumerable<KiteschoolReadDto>> GetKiteschoolsByUserId(int userId);
    }
}