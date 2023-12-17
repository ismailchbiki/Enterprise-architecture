using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Dtos;
using UserService.SyncDataServices.Http;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KiteschoolController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IKiteschoolDataClient _kiteschoolDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public KiteschoolController(
            IMapper mapper,
            IKiteschoolDataClient kiteschoolDataClient,
            IMessageBusClient messageBusClient)
        {
            _mapper = mapper;
            _kiteschoolDataClient = kiteschoolDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpPost]
        public ActionResult<KiteschoolReadDto> CreateKiteschool(KiteschoolCreateDto kiteschoolCreateDto)
        {
            Console.WriteLine($"--> Creating Kiteschool in Kiteschool Service...");

            try
            {
                // Send via message bus
                SendViaMessageBus(kiteschoolCreateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing event: {ex.Message}");
                return StatusCode(500, "Error publishing event");
            }
        }

        private void SendViaMessageBus(KiteschoolCreateDto kiteschoolCreateDto)
        {
            try
            {
                var kiteschoolPublishedDto = _mapper.Map<KiteschoolPublishedDto>(kiteschoolCreateDto);

                // Assign event name to the property
                kiteschoolPublishedDto.Event = "Kiteschool_Published";

                // Publish the event to the MessageBus
                _messageBusClient.PublishNewKiteschool(kiteschoolPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }
        }

        [HttpGet("{userId}", Name = "GetKiteschoolsByUserIdAsync")]
        public async Task<ActionResult<IEnumerable<KiteschoolReadDto>>> GetKiteschoolsByUserIdAsync(int userId)
        {
            try
            {
                var kiteschools = await _kiteschoolDataClient.GetKiteschoolsByUserId(userId);

                if (kiteschools != null)
                {
                    Console.WriteLine($"--> Received {kiteschools.Count()} kiteschools.");
                    return Ok(kiteschools);
                }
                else
                {
                    Console.WriteLine($"No kite schools found for user with ID {userId}");
                    return NotFound($"No kite schools found for user with ID {userId}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in external HTTP request: {ex.Message}");
                return StatusCode(500, $"Error in external HTTP request: {ex.Message} Please try again later.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while fetching kite schools.");
            }
        }

        // Send a direct POST request to KiteschoolService endpoint
        // private void SendDirectHttpPostRequest(KiteschoolReadDto kiteschoolReadDto)
        // {
        //     // Send the new kiteschool object to the KiteschoolService (direct http post request)
        //     try
        //     {
        //         _kiteschoolDataClient.SendKiteschoolToKiteschoolService(kiteschoolReadDto);
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        //     }
        // }
    }
}