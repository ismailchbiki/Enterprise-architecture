using AutoMapper;
using KiteschoolService.Data;
using KiteschoolService.Dtos;
using KiteschoolService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiteschoolService.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
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

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<KiteschoolReadDto>> GetKiteschoolsByUserId(int userId)
        {
            Console.WriteLine($"--> Getting Kite schools by User Id: {userId}...");

            var kiteschools = _kiteschoolRepo.GetKiteschoolsByUserId(userId);

            if (kiteschools == null || !kiteschools.Any())
            {
                return NotFound($"No kite schools found for user with ID {userId}");
            }

            return Ok(kiteschools);
        }

        [HttpGet("{id}", Name = "GetKiteschoolById")]
        public ActionResult<KiteschoolReadDto> GetKiteschoolById(string id)
        {
            Console.WriteLine($"--> Getting Kiteschool by Id: {id}...");

            try
            {
                var kiteschoolItem = _kiteschoolRepo.GetKiteschoolById(id);

                if (kiteschoolItem != null)
                {
                    return Ok(_mapper.Map<KiteschoolReadDto>(kiteschoolItem));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving the Kiteschool. Error message: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateKiteschool([FromBody] KiteschoolCreateDto kiteschoolDto)
        {
            try
            {
                if (kiteschoolDto == null)
                {
                    return BadRequest("Kiteschool data is null.");
                }

                var kiteschool = _mapper.Map<Kiteschool>(kiteschoolDto);
                _kiteschoolRepo.CreateKiteschool(kiteschool);

                return CreatedAtRoute("GetKiteschoolById", new { id = kiteschool.Id }, _mapper.Map<KiteschoolReadDto>(kiteschool));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while creating the Kiteschool. Error message: " + ex.Message);
            }
        }
    }
}