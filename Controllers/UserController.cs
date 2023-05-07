using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private BambooContext db;
        private IMapper _mapper;
        public UserController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] AddUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            User exists = db.Users.Where(b => b.userName == user.userName).FirstOrDefault();
            if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
            db.Users.Add(user);
            db.SaveChanges();
            return CreatedAtAction(nameof(user), user);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(db.Users);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = db.Users.FirstOrDefault(b => b.userID.Equals(id));
            if (user == null) { return NotFound(); }
            return Ok(user);
        }

    }
}
