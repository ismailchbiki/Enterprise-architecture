using System.Text.Json;
using AutoMapper;
using KiteschoolService.Data;
using KiteschoolService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace KiteschoolService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class KiteschoolController : ControllerBase
    {
        private readonly IKiteschoolRepo _kiteschoolRepo;
        private readonly IMapper _mapper;

        public KiteschoolController(IKiteschoolRepo kiteschoolRepo, IMapper mapper)
        {
            _kiteschoolRepo = kiteschoolRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KiteschoolReadDto>> GetKiteschools()
        {
            Console.WriteLine("--> Getting Kite schools from Kiteschool Service");

            var kiteschoolItems = _kiteschoolRepo.GetAllKiteschools();

            return Ok(_mapper.Map<IEnumerable<KiteschoolReadDto>>(kiteschoolItems));
        }

        [HttpPost]
        public ActionResult CreateKiteschool([FromBody] KiteschoolReadDto kiteschool)
        {
            Console.WriteLine("--> A post request is made from UserService");
            Console.WriteLine($"Received Kiteschool Object: {JsonSerializer.Serialize(kiteschool)}");

            // Perform necessary actions with the received kiteschool object

            return Ok("Response from Kiteshcool Service to User Service");
        }

    }
}