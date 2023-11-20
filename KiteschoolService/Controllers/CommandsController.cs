using AutoMapper;
using KiteschoolService.Data;
using KiteschoolService.Dtos;
using KiteschoolService.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiteschoolService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IKiteschoolRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(IKiteschoolRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        // {
        //     Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

        //     if (!_repository.KiteschoolExists(platformId))
        //     {
        //         return NotFound();
        //     }

        //     var commands = _repository.GetCommandsForPlatform(platformId);

        //     return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        // }

        // [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        // public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        // {
        //     Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        //     if (!_repository.KiteschoolExists(platformId))
        //     {
        //         return NotFound();
        //     }

        //     var command = _repository.GetCommand(platformId, commandId);

        //     if (command == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(_mapper.Map<CommandReadDto>(command));
        // }

        // [HttpPost]
        // public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        // {
        //     Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        //     if (!_repository.KiteschoolExists(platformId))
        //     {
        //         return NotFound();
        //     }

        //     var command = _mapper.Map<Command>(commandDto);

        //     _repository.CreateCommand(platformId, command);
        //     _repository.SaveChanges();

        //     var commandReadDto = _mapper.Map<CommandReadDto>(command);

        //     return CreatedAtRoute(nameof(GetCommandForPlatform),
        //         new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        // }
    }
}