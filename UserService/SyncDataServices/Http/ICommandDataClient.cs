using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendKiteschoolToCommand(KiteschoolReadDto plat);
    }
}