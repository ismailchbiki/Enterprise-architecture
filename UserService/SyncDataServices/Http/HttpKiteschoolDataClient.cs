using System.Text;
using System.Text.Json;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    // This class is responsible for direct communication with the KiteschoolService (without a queue or message broker)
    public class HttpKiteschoolDataClient : IKiteschoolDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpKiteschoolDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Send KiteschoolReadDto object to the KiteschoolService synchronously (not via a queue or message broker)
        // Even if the method is async, the communication with the KiteschoolService is still synchronous because it's a direct communication with the other service.
        public async Task SendKiteschoolToKiteschoolService(KiteschoolReadDto kiteschool)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(kiteschool),
                Encoding.UTF8,
                "application/json"
            );

            // Send a POST request to KiteschoolService endpoint (direct contact with the other service)
            var response = await _httpClient.PostAsync($"{_config["KiteschoolService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Kiteschool Service was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Kiteschool Service was NOT OK!");
            }

            // Read the response content from kiteschool service as a string
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");
        }
    }
}