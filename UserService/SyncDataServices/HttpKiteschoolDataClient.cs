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

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not fetch kiteschools for user with id {userId} from kiteschool service: {ex.Message}");
                return null;
            }
        }
    }
}