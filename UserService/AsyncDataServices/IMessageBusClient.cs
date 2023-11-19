using UserService.Dtos;

namespace UserService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewKiteschool(KiteschoolPublishedDto kiteschoolPublishedDto);
    }
}