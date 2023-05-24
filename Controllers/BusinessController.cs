using AutoMapper;
using Bamboo.API;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
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

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        public BusinessController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("AddBusiness")]
        public async Task<IActionResult> AddBusiness([FromBody] AddBusinessDto businessDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Business business = mapper.Map<Business>(businessDto);
                Business exists = db.Businesses.Where(b => b.name == business.name).FirstOrDefault();
                if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
                if (business.logoUrl == null)
                {
                    GoogleImage googleImage = new GoogleImage();
                    string response = await googleImage.Search(business.name + "," + business.description);
                    business.logoUrl = response;
                }
                db.Businesses.Add(business);
                db.SaveChanges();
                return CreatedAtAction(nameof(business), business);
            }
            else { return Unauthorized("Please login to add a business!"); }
        }

        [HttpPost("RemoveBusiness/{businessID}")]
        public IActionResult RemoveBusiness(Guid businessID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db);
                if (!loggedUser.ownerOfBusinessID.Equals(businessID)) return BadRequest("You must be the owner of the business to remove it!");
                Business dbBusiness = db.Businesses.Where(a => a.businessID.Equals(businessID)).FirstOrDefault();
                if (dbBusiness == null) { return Ok(); }
                List<Product> businessProducts = db.Products.Where(p => p.businessID.Equals(businessID)).ToList();
                db.Products.RemoveRange(businessProducts);
                db.Businesses.Remove(dbBusiness);
                db.SaveChanges();
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditBusiness/{businessId}")]
        public IActionResult EditBusiness(Guid businessID,[FromBody] EditBusinessDto businessDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db);
                if (!loggedUser.ownerOfBusinessID.Equals(businessID)) return BadRequest("You must be the owner of the business to edit it!");
                Business dbBusiness = db.Businesses.Where(b => b.businessID == businessID).FirstOrDefault();
                if (dbBusiness == null) return NotFound();
                Business businessNewInfo = mapper.Map<Business>(businessDto);

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
            else { return Unauthorized(); }
        }

        [HttpGet("GetBusinesses")]
        public IActionResult GetBusinesses()
        {
            return Ok(db.Businesses);
        }

        [HttpGet("ListBusinesses")]
        public IActionResult ListBusinesses()
        {
            List<ReadBusinessDto> readBusinessDtos = mapper.Map<List<ReadBusinessDto>>(db.Businesses.ToList());
            return Json(readBusinessDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
