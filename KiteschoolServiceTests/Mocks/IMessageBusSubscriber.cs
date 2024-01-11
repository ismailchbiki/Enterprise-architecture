using RabbitMQ.Client;

namespace KiteschoolServiceTests.Mocks
{
    public interface IMessageBusSubscriber
    {
        void InitializeRabbitMQ();
        void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e);
    }
}