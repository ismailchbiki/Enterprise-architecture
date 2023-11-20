using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;
using UserService.SyncDataServices.Http;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IKiteschoolRepo _repository;
        private readonly IMapper _mapper;
        private readonly IKiteschoolDataClient _kiteschoolDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UserController(
            IKiteschoolRepo repository,
            IMapper mapper,
            IKiteschoolDataClient kiteschoolDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _kiteschoolDataClient = kiteschoolDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KiteschoolReadDto>> GetKiteschools()
        {
            Console.WriteLine("--> Getting Kiteschools...");

            var kiteschoolItems = _repository.GetAllKiteschools();

            return Ok(_mapper.Map<IEnumerable<KiteschoolReadDto>>(kiteschoolItems));
        }

        [HttpGet("{id}", Name = "GetKiteschoolById")]
        public ActionResult<KiteschoolReadDto> GetKiteschoolById(int id)
        {
            Console.WriteLine($"--> Getting Kiteschool by Id: {id}...");

            var kiteschoolItem = _repository.GetKiteschoolById(id);

            if (kiteschoolItem != null)
            {
                return Ok(_mapper.Map<KiteschoolReadDto>(kiteschoolItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<KiteschoolReadDto>> CreateKiteschool(KiteschoolCreateDto kiteschoolCreateDto)
        {
            Console.WriteLine($"--> Creating Kiteschool...");

            // Map kiteschool object and save it to DB
            var kiteschoolModel = _mapper.Map<Kiteschool>(kiteschoolCreateDto);
            _repository.CreateKiteschool(kiteschoolModel);
            // _repository.SaveChanges();

            // Get the newly added kiteschool object from DB for further processing
            var kiteschoolReadDto = _mapper.Map<KiteschoolReadDto>(kiteschoolModel);

            // Direct post request
            SendDirectHttpPostRequest(kiteschoolReadDto);

            // Send via message bus
            SendViaMessageBus(kiteschoolReadDto);

            return CreatedAtRoute(nameof(GetKiteschoolById), new { Id = kiteschoolReadDto.Id }, kiteschoolReadDto);
        }

        private void SendDirectHttpPostRequest(KiteschoolReadDto kiteschoolReadDto)
        {
            // Send the new kiteschool object to the KiteschoolService (direct http post request)
            try
            {
                _kiteschoolDataClient.SendKiteschoolToKiteschoolService(kiteschoolReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
        }

        private void SendViaMessageBus(KiteschoolReadDto kiteschoolReadDto)
        {

            /*
                Send the new kiteschool object to the KiteschoolService (event-driven architecture using a MessageBus)
                Asynchronous event publishing
            */
            try
            {
                var kiteschoolPublishedDto = _mapper.Map<KiteschoolPublishedDto>(kiteschoolReadDto);

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
    }
}