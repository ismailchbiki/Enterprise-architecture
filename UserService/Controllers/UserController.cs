using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;

        public UserController(
            IUserRepo repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateUserAsync(UserCreateDto userCreateDto)
        {
            Console.WriteLine($"--> Creating User...");

            // Create new user
            var userModel = _mapper.Map<User>(userCreateDto);
            await _repository.CreateUserAsync(userModel);
            await _repository.SaveChangesAsync();

            // Get the newly added user object from DB for further processing
            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            return CreatedAtRoute(nameof(GetUserByIdAsync), new { Id = userReadDto.Id }, userReadDto);
        }

        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        public async Task<ActionResult<UserReadDto>> GetUserByIdAsync(int id)
        {
            Console.WriteLine($"--> Getting user by Id: {id}...");

            var userItem = await _repository.GetUserByIdAsync(id);

            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsersAsync()
        {
            Console.WriteLine("--> Getting users...");

            var userItems = await _repository.GetAllUsersAsync();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }
    }
}