﻿using AutoMapper;
using Bamboo.API;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        public ProductController(BambooContext context, IMapper _mapper, TokenValidator _tokenValidator)
        {
            db = context;
            mapper = _mapper;
            tokenValidator = _tokenValidator;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto productDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Product product = mapper.Map<Product>(productDto);
                Product exists = db.Products.Where(p => p.name == product.name).FirstOrDefault();
                if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
                if (product.imageUrl == null)
                {
                    GoogleImage googleImage = new GoogleImage();
                    string response = await googleImage.Search(product.name+","+product.description);
                    product.imageUrl = response;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return CreatedAtAction(nameof(product), product);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveProduct/{productID}")]
        public IActionResult RemoveProduct(Guid productID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Product dbProduct = db.Products.Where(a => a.productID.Equals(productID)).FirstOrDefault();
                if (dbProduct == null) { return Ok(); }
                db.Products.Remove(dbProduct);
                db.SaveChanges();
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditProduct/{productID}")]
        public IActionResult EditProduct(Guid productID,[FromBody] EditProductDto productDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Product dbProduct = db.Products.Where(p => p.productID == productID).FirstOrDefault();
                if (dbProduct == null) return NotFound();
                Product productNewInfo = mapper.Map<Product>(productDto);

                PropertyInfo[] properties = productNewInfo.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(productNewInfo) != null && property.Name != "productID")
                    {
                        property.SetValue(dbProduct, property.GetValue(productNewInfo));
                    }
                }

                db.Entry(dbProduct).State = EntityState.Modified;
                db.SaveChanges();
                return CreatedAtAction(nameof(dbProduct), dbProduct);
            }
            else { return Unauthorized(); }
        }

        [HttpGet("ListProducts")]
        public IActionResult ListProducts()
        {
            List<ReadProductDto> readProductsDtos = mapper.Map<List<ReadProductDto>>(db.Products.ToList());
            return Json(readProductsDtos);
        }


        [HttpGet("ListProductsOfBusiness/{businessID}")]
        public IActionResult ListProductsOfBusiness(Guid businessID)
        {
            List<ReadProductDto> readProductsDtos = mapper.Map<List<ReadProductDto>>(db.Products.Where(p => p.businessID.Equals(businessID)).ToList());
            return Json(readProductsDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
