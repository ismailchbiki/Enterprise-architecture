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
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UserController(
            IKiteschoolRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
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

            // Dto -> Model/entity conversion
            var kiteschoolModel = _mapper.Map<Kiteschool>(kiteschoolCreateDto);

            // Add new Kiteschool to DB
            _repository.CreateKiteschool(kiteschoolModel);
            _repository.SaveChanges();

            // Get result from DB
            var kiteschoolReadDto = _mapper.Map<KiteschoolReadDto>(kiteschoolModel);

            // Send Sync Message (direct http post request)
            try
            {
                // Send kiteschoolReadDtoReadDto object to the CommandService
                // (through an HTTP POST request to the CommandService endpoint)
                await _commandDataClient.SendKiteschoolToCommand(kiteschoolReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            /*
            The below code is an asynchronous message publishing operation.
            This is part of a messaging system or event-driven architecture.

            It is responsible for asynchronously publishing a message,
            related to the publication of a new kiteschool, to a message bus or broker.
            */

            // Send Async Message (via a MessageBus or Broker - RabbitMQ)
            try
            {
                var kiteschoolPublishedDto = _mapper.Map<KiteschoolPublishedDto>(kiteschoolReadDto);
                // Normally the event(s) need to be documented about the entire Microservice Architecture
                // Like a documented library of events (expected to be sent and received)

                // Assign event name to the property
                kiteschoolPublishedDto.Event = "Kiteschool_Published";

                // Publish the event to the MessageBus
                _messageBusClient.PublishNewKiteschool(kiteschoolPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetKiteschoolById), new { Id = kiteschoolReadDto.Id }, kiteschoolReadDto);
        }
    }
}