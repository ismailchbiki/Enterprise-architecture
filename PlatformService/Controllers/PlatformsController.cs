using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            IPlatformRepo repository,
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
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine($"--> Getting Platform by Id: {id}...");

            var platformItem = _repository.GetPlatformById(id);

            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine($"--> Creating Platform...");

            // Dto -> Model/entity conversion
            var platformModel = _mapper.Map<Platform>(platformCreateDto);

            // Create new Platform to DB
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            // Get result from DB
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Sync Message (direct http post request)
            try
            {
                // Send PlatformReadDto object to the CommandService
                // (through an HTTP POST request to the CommandService endpoint)
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            /*
            The below code is an asynchronous message publishing operation.
            This is part of a messaging system or event-driven architecture.

            It is responsible for asynchronously publishing a message,
            related to the publication of a new platform, to a message bus or broker.
            */

            // Send Async Message (via a MessageBus or Broker - RabbitMQ)
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                // Normally the event(s) need to be documented about the entire Microservice Architecture
                // Like a documented library of events (expected to be sent and received)

                // Assign event name to the property
                platformPublishedDto.Event = "Platform_Published";

                // Publish the event to the MessageBus
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePlatform(int id)
        {
            Console.WriteLine($"--> Updating Platform with Id: {id}...");

            var platformModelFromRepo = _repository.GetPlatformById(id);

            if (platformModelFromRepo == null)
            {
                return NotFound();
            }

            // Dto -> Model/entity conversion
            // _mapper.Map(platformUpdateDto, platformModelFromRepo);

            // Update Platform to DB
            // _repository.UpdatePlatform(platformModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}