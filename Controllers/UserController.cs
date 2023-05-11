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
    public class UserController : Controller
    {
        private BambooContext db;
        private IMapper _mapper;
        private UserService _userService;

        public UserController(BambooContext context, IMapper mapper, UserService userService)
        {
            db = context;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
        {
            await _userService.RegisterAsync(userDto);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            var token = await _userService.LoginAsync(userDto);
            return Json(token);        
        }

        [HttpPost("Logoff")]
        public async Task<IActionResult> Logoff()
        {
            bool result = await _userService.LogoffAsync();
            return Ok(result);
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

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
