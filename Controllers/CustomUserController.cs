﻿using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System;
using Bamboo.Services;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomUserController : Controller
    {
        public string customKey = "XLIKkMfhk38hUzOSjXiOdsWvKj6KEz5?U6kZKnYnYs8bd1JTTz-5b-js2G";
        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        private IWebHostEnvironment hostingEnvironment;
        private LogService logManager;
        public CustomUserController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.hostingEnvironment = hostingEnvironment;
            this.logManager = logManager;
        }

        public string GenerateToken(Guid userID)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(customKey+userID.ToString()));

                Guid guid = new Guid(data.Take(16).ToArray());

                return guid.ToString();
            }
        }

        public string CreateHashedPassWord(string password)
        {
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltSize: 16, iterations: 10000))
            {
                byte[] salt = rfc2898DeriveBytes.Salt;
                byte[] hash = rfc2898DeriveBytes.GetBytes(20); 

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                string hashedPassword = Convert.ToBase64String(hashBytes);

                return hashedPassword;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations: 10000))
            {
                byte[] hash = rfc2898DeriveBytes.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [HttpPost("AddUser")]
        public IActionResult AddCustomUser([FromBody] AddCustomUserDto userDto)
        {
            CustomUser dbUser = new CustomUser();
            dbUser.role = userDto.ownerOfBusinessID.HasValue ? "BusinessOwner" : "Consumer";
            CustomUser userNewInfo = mapper.Map<CustomUser>(userDto);

            PropertyInfo[] properties = userNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(userNewInfo) != null && property.Name != "userID")
                {
                    property.SetValue(dbUser, property.GetValue(userNewInfo));
                }
            }

            dbUser.userPassword = CreateHashedPassWord(userDto.userPassword);
            string emailTokenConfirmation = new Guid().ToString();
            dbUser.emailTokenConfirmation = emailTokenConfirmation;

            db.CustomUsers.Add(dbUser);
            db.SaveChanges();
            logManager.AddLog($"User: {dbUser.userFirstName} id=({dbUser.userID}) added successfully!");
            //string url = "https://bamboo.ngrok.app" + "/ConfirmEmail/" + emailTokenConfirmation;
            //var gmailService = new EmailApi();
            //gmailService.sendEmailAsync(dbUser.userEmail, "Confirm your email for Bamboo", url);
            return Ok();
        }

        [HttpGet("ConfirmEmail/{emailTokenConfirmation}")]
        public IActionResult ConfirmEmail(string emailTokenConfirmation) 
        {
            CustomUser dbUser = db.CustomUsers.Where(u => u.emailTokenConfirmation.Equals(emailTokenConfirmation)).FirstOrDefault();
            if (dbUser == null) { return BadRequest(); }
            dbUser.isActive = true;
            db.Entry(dbUser).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginCustomUserDto userDto)
        {
            CustomUser dbUser = db.CustomUsers.Where(u => u.userName.Equals(userDto.userName)).FirstOrDefault();
            if (dbUser == null) { return  BadRequest(); }
            bool authorized = VerifyPassword(userDto.userPassword, dbUser.userPassword);
            if (authorized)
            {
                string token = GenerateToken(dbUser.userID);
                DateTime tokenExpirationDate = DateTime.Now.AddMinutes(30);
                dbUser.token = token;
                dbUser.tokenExpirationDate = tokenExpirationDate;
                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                logManager.AddLog($"User: {dbUser.userFirstName} id=({dbUser.userID}) logged in successfully!");
                return Json(token);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                dbUser.tokenExpirationDate = DateTime.Now;
                if (dbUser == null ) { return Unauthorized(); }
                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                logManager.AddLog($"User: {dbUser.userFirstName} id=({dbUser.userID}) logged out successfully!");
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditCustomUser/{userID}")]
        public IActionResult EditUser(Guid userID, [FromBody] EditCustomUserDto userDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized) 
            {
                CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db); if (loggedUser == null) return NotFound();
                CustomUser userNewInfo = mapper.Map<CustomUser>(userDto);

                PropertyInfo[] properties = userNewInfo.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(userNewInfo) != null && property.Name != "userID")
                    {
                        property.SetValue(loggedUser, property.GetValue(userNewInfo));
                    }
                    if (property.Name == "ownerOfBusinessID")
                    {
                        property.SetValue(loggedUser, property.GetValue(userNewInfo));
                    }
                }

                db.Entry(loggedUser).State = EntityState.Modified;
                db.SaveChanges();
                logManager.AddLog($"User: {loggedUser.userFirstName} id=({loggedUser.userID}) edited successfully!");
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveCustomUser/{userID}")]
        public IActionResult RemoveUser(Guid userID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                CustomUser dbUser = db.CustomUsers.Where(a => a.userID.Equals(userID)).FirstOrDefault();
                if (dbUser == null) return NotFound();

                db.Remove(dbUser);
                db.SaveChanges();
                logManager.AddLog($"User: {dbUser.userFirstName} id=({dbUser.userID}) removed successfully!");
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpGet("ListUsers")]
        public IActionResult ListUsers()
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                ReadCustomUserDto dbUserDto = mapper.Map<ReadCustomUserDto>( db.CustomUsers.Where(u => u.token.Equals(token) && u.role != "Admin").FirstOrDefault());
                if (dbUserDto.ownerOfBusinessID.HasValue)
                {
                    ReadBusinessDto dbBusiness = mapper.Map<ReadBusinessDto>(db.Businesses.Where(b => b.businessID.Equals(dbUserDto.ownerOfBusinessID)).FirstOrDefault());
                    dbUserDto.businessDto = dbBusiness;
                }
                if (dbUserDto == null) return NotFound();
                List<ReadCustomUserDto> dbUsers = new List<ReadCustomUserDto>();
                dbUsers.Add(dbUserDto);
                return Json(dbUsers);
            }
            else { return Unauthorized(); }
        }

        [HttpGet("CheckToken")]
        public IActionResult CheckToken()
        {
            bool authorized = tokenValidator.ValidateToken();
            if(authorized) { return Ok(); }
            return Unauthorized();
        }

        [HttpGet("GetOwnerOfBusinessID")]
        public IActionResult GetOwnerOfBusinessID()
        {
            CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db);
            return Json(loggedUser.ownerOfBusinessID);
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
