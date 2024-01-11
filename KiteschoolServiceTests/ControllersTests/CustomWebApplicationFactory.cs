using KiteschoolService.Data;
using KiteschoolServiceTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace KiteschoolServiceTests.ControllersTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IKiteschoolRepo> MockKiteschoolRepo { get; }
        Mock<IMessageBusSubscriber> MockMessageBusSubscriber { get; }



        public CustomWebApplicationFactory()
        {
            MockKiteschoolRepo = new Mock<IKiteschoolRepo>();
            MockMessageBusSubscriber = new Mock<IMessageBusSubscriber>();
            MockMessageBusSubscriber.Setup(m => m.InitializeRabbitMQ()).Verifiable();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.AddControllers().AddNewtonsoftJson();
                services.AddSingleton(MockKiteschoolRepo.Object);
            });
        }
    }
}
