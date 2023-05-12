using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return Ok();
        }

        [HttpPost("Logoff")]
        public async Task<IActionResult> Logoff()
        {
            bool result = await _userService.LogoffAsync();
            return Ok(result);
        }

        [HttpPost("EditUser/{userID}")]
        public IActionResult EditUser(Guid userID, [FromBody] EditUserDto userDto)
        {
            User dbUser = db.Users.Where(a => a.Id.Equals(userID)).FirstOrDefault();
            if (dbUser == null) return NotFound();
            User userNewInfo = _mapper.Map<User>(userDto);

            PropertyInfo[] properties = userNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(userNewInfo) != null && property.Name != "Id")
                {
                    property.SetValue(dbUser, property.GetValue(userNewInfo));
                }
            }

            db.Entry(dbUser).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtAction(nameof(dbUser), dbUser);
        }

        [HttpGet("ListUsers")]
        public IActionResult ListUsers()
        {
            List<ReadUserDto> readUserDtos = _mapper.Map<List<ReadUserDto>>(db.Users.ToList());
            return Json(readUserDtos);
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

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
