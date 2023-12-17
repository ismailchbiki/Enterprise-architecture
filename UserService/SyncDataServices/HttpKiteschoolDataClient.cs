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

        // Fetch Kiteschools by UserId from kiteschoolService with a direct http get request
        public async Task<IEnumerable<KiteschoolReadDto>> GetKiteschoolsByUserId(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_config["KiteschoolService"]}user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<KiteschoolReadDto>>();
                    return content;
                }
                else
                {
                    var errorMessage = $"Failed to fetch kiteschools for user with id {userId}. Status code: {response.StatusCode}";
                    Console.WriteLine($"--> {errorMessage}");
                    throw new HttpRequestException(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"--> HTTP Request Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unexpected error while fetching kiteschools for user with id {userId}: {ex.Message}";
                Console.WriteLine($"--> {errorMessage}");
                throw new Exception(errorMessage, ex);
            }
        }
    }
}