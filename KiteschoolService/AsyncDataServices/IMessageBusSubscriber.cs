using RabbitMQ.Client;

namespace KiteschoolService.AsyncDataServices
{
    public interface IMessageBusSubscriber
    {
        void InitializeRabbitMQ();
        void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e);
    }

}