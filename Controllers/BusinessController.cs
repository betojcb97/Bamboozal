using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : Controller
    {

        private BambooContext db;
        private IMapper _mapper;
        public BusinessController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost("AddBusiness")]
        public IActionResult AddBusiness([FromBody] AddBusinessDto businessDto)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                if (dbUser == null || dbUser.tokenExpirationDate < DateTime.Now) { return Unauthorized(); }
                Business business = _mapper.Map<Business>(businessDto);
                Business exists = db.Businesses.Where(b => b.name == business.name).FirstOrDefault();
                if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
                db.Businesses.Add(business);
                db.SaveChanges();
                return CreatedAtAction(nameof(business), business);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("RemoveBusiness/{businessID}")]
        public IActionResult RemoveBusiness(Guid businessID)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                if (dbUser == null || dbUser.tokenExpirationDate < DateTime.Now) { return Unauthorized(); }
                Business dbBusiness = db.Businesses.Where(a => a.businessID.Equals(businessID)).FirstOrDefault();
                if (dbBusiness == null) { return Ok(); }
                db.Businesses.Remove(dbBusiness);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("EditBusiness/{businessId}")]
        public IActionResult EditBusiness(Guid businessID,[FromBody] EditBusinessDto businessDto)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                if (dbUser == null || dbUser.tokenExpirationDate < DateTime.Now) { return Unauthorized(); }
                Business dbBusiness = db.Businesses.Where(b => b.businessID == businessID).FirstOrDefault();
                if (dbBusiness == null) return NotFound();
                Business businessNewInfo = _mapper.Map<Business>(businessDto);

                PropertyInfo[] properties = businessNewInfo.GetType().GetProperties();

                foreach(PropertyInfo property in properties)
                {
                    if (property.GetValue(businessNewInfo) != null && property.Name != "businessID")
                    {
                        property.SetValue(dbBusiness, property.GetValue(businessNewInfo));
                    }
                }

                db.Entry(dbBusiness).State = EntityState.Modified;
                db.SaveChanges();
                return CreatedAtAction(nameof(dbBusiness), dbBusiness);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("GetBusinesses")]
        public IActionResult GetBusinesses()
        {
            return Ok(db.Businesses);
        }

        [HttpGet("ListBusinesses")]
        public IActionResult ListBusinesses()
        {
            List<ReadBusinessDto> readBusinessDtos = _mapper.Map<List<ReadBusinessDto>>(db.Businesses.ToList());
            return Json(readBusinessDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetBusinessById(Guid id)
        {
            var business = db.Businesses.FirstOrDefault(b => b.businessID.Equals(id));
            if (business == null) { return NotFound(); }
            return Ok(business);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
