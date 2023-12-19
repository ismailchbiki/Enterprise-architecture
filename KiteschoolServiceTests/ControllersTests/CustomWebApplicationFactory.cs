using KiteschoolService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace KiteschoolServiceTests.ControllersTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IKiteschoolRepo> MockKiteschoolRepo { get; }

        public CustomWebApplicationFactory()
        {
            MockKiteschoolRepo = new Mock<IKiteschoolRepo>();
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
