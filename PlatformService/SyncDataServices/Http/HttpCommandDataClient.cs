using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    // This class is responsible for direct communication with the CommandService (without a queue or message broker)
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Send PlatformReadDto object to the CommandService synchronously (not via a queue or message broker)
        // Even if the method is async, the communication with the CommandService is still synchronous because it's a direct communication with the other service.
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );

            // Send a POST request to the CommandService endpoint (direct contact with the other service)
            var response = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Command Service was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Command Service was NOT OK!");
            }
        }
    }
}