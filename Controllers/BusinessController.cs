﻿using AutoMapper;
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
        public BusinessController(BambooContext context, IMapper _mapper, TokenValidator _tokenValidator)
        {
            db = context;
            mapper = _mapper;
            tokenValidator = _tokenValidator;
        }

        [HttpPost("AddBusiness")]
        public IActionResult AddBusiness([FromBody] AddBusinessDto businessDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Business business = mapper.Map<Business>(businessDto);
                Business exists = db.Businesses.Where(b => b.name == business.name).FirstOrDefault();
                if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
                db.Businesses.Add(business);
                db.SaveChanges();
                return CreatedAtAction(nameof(business), business);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveBusiness/{businessID}")]
        public IActionResult RemoveBusiness(Guid businessID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
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
