using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomUserController : Controller
    {
        public string customKey = "XLIKkMfhk38hUzOSjXiOdsWvKj6KEz5?U6kZKnYnYs8bd1JTTz-5b-js2G";
        private BambooContext db;
        private IMapper _mapper;

        public string generateToken(Guid userID)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(customKey+userID.ToString()));

                Guid guid = new Guid(data.Take(16).ToArray());

                return guid.ToString();
            }
        }

        public CustomUserController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost("AddUser")]
        public IActionResult AddCustomUser([FromBody] AddCustomUserDto userDto)
        {
            CustomUser dbUser = new CustomUser();
            CustomUser userNewInfo = _mapper.Map<CustomUser>(userDto);

            PropertyInfo[] properties = userNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(userNewInfo) != null && property.Name != "userID")
                {
                    property.SetValue(dbUser, property.GetValue(userNewInfo));
                }
            }
            db.CustomUsers.Add(dbUser);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginCustomUserDto userDto)
        {
            CustomUser dbUser = db.CustomUsers.Where(u => u.userName.Equals(userDto.userName) && u.userPassword.Equals(userDto.userPassword)).FirstOrDefault();
            if (dbUser == null) { return  BadRequest(); }
            string token = generateToken(dbUser.userID);
            DateTime tokenExpirationDate = DateTime.Now.AddMinutes(30);
            dbUser.token = token;
            dbUser.tokenExpirationDate = tokenExpirationDate;
            db.Entry(dbUser).State = EntityState.Modified;
            db.SaveChanges();
            return Json(token);
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                if (dbUser == null ) { return Unauthorized(); }
                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("EditCustomUser")]
        public IActionResult EditUser([FromBody] EditCustomUserDto userDto)
        {
            CustomUser dbUser = db.CustomUsers.Where(a => a.userName.Equals(userDto.userName)).FirstOrDefault();
            if (dbUser == null) return NotFound();
            CustomUser userNewInfo = _mapper.Map<CustomUser>(userDto);

            PropertyInfo[] properties = userNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(userNewInfo) != null && property.Name != "userID")
                {
                    property.SetValue(dbUser, property.GetValue(userNewInfo));
                }
            }

            db.Entry(dbUser).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpGet("ListUsers")]
        public IActionResult ListUsers()
        {
            List<ReadCustomUserDto> readUserDtos = _mapper.Map<List<ReadCustomUserDto>>(db.CustomUsers.ToList());
            return Json(readUserDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            CustomUser user = db.CustomUsers.FirstOrDefault(b => b.userID.Equals(id));
            if (user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpGet("LoginPage")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet("LogoutPage")]
        public IActionResult LogoutPage()
        {
            return View();
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
