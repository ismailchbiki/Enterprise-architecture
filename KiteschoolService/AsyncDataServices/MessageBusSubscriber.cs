using System.Text;
using KiteschoolService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KiteschoolService.AsyncDataServices
{
    /*
        This class will be responsible for subscribing to the message bus
        and listening for events that are published to the message bus.
        When an event is published to the message bus, this class will
        receive that event and then do something with it.
    */
    public class MessageBusSubscriber : BackgroundService, IMessageBusSubscriber
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        // Create the connection to the Message Bus
        public void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["RabbitMQHost"],
                    Port = int.Parse(_configuration["RabbitMQPort"])
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _queueName = _channel.QueueDeclare().QueueName;

                _channel.QueueBind(
                    queue: _queueName,
                    exchange: "trigger",
                    routingKey: "");

                Console.WriteLine("--> Listening on the Message Bus...");

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing RabbitMQ: {ex.Message}");
                // Handle the error according to your requirements
            }
        }

        public void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
            // Handle the shutdown event if needed
        }

        // BackgroundService
        public override void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");

            if (_channel?.IsOpen ?? false)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }

        // This will consume the message from the message bus
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            // Check if RabbitMQ connection is available before proceeding
            if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ is not available. MessageBusSubscriber will not process messages.");
                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            // Subscribe to the event
            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                // Process the event (add published kiteschool to the database)
                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
