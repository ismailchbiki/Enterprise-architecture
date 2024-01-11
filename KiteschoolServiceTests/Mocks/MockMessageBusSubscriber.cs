using KiteschoolService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace KiteschoolServiceTests.Mocks
{
    public class MockMessageBusSubscriber : BackgroundService, IMessageBusSubscriber
    {
        private readonly IEventProcessor _eventProcessor;

        public MockMessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
        }

        public void InitializeRabbitMQ()
        {
            Console.WriteLine("MockMessageBusSubscriber: RabbitMQ initialized successfully (mocked).");
        }

        // Simulate RabbitMQ connection shutdown
        public void RabbitMQ_ConnectionShutdown(object sender, RabbitMQ.Client.ShutdownEventArgs e)
        {
            Console.WriteLine($"MockMessageBusSubscriber: RabbitMQ connection shutdown. Reason: {e.ReplyText}");
        }

        public override void Dispose()
        {
            Console.WriteLine("MockMessageBusSubscriber: Disposing resources.");
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitializeRabbitMQ();  // Simulate RabbitMQ initialization

            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate receiving a message
                var message = "Sample message";

                // Process events using the mocked event processor
                _eventProcessor.ProcessEvent(message);

                // Simulate some delay before processing the next batch of events
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}