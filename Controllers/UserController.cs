using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private BambooContext db;
        private IMapper _mapper;
        private RegisterUser _registerUserService;

        public UserController(BambooContext context, IMapper mapper, RegisterUser registerUserService)
        {
            db = context;
            _mapper = mapper;
            _registerUserService = registerUserService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
        {
            await _registerUserService.RegisterAsync(userDto);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(db.Users);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = db.Users.FirstOrDefault(b => b.Id.Equals(id));
            if (user == null) { return NotFound(); }
            return Ok(user);
        }

    }
}
