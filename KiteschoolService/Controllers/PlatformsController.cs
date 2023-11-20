using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");

            var platformItems = _commandRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult CreateKiteschool([FromBody] KiteschoolReadDto kiteschool)
        {
            // This method could receive the kiteschool data and insert it to db
            Console.WriteLine("--> A post request is made from UserService");
            Console.WriteLine($"Received Kiteschool Object: {JsonSerializer.Serialize(kiteschool)}");

            // Perform necessary actions with the received kiteschool object

            return Ok("Response from Kiteshcool Service to User Service");
        }

    }
}