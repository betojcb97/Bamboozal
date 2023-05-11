using AutoMapper;
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

        private BambooContext db;
        private IMapper _mapper;
        public ProductController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody] AddProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            Product exists = db.Products.Where(p => p.name == product.name).FirstOrDefault();
            if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
            db.Products.Add(product);
            db.SaveChanges();
            return CreatedAtAction(nameof(product), product);
        }

        [HttpPost("RemoveProduct/{productID}")]
        public IActionResult RemoveProduct(Guid productID)
        {
            Product dbProduct = db.Products.Where(a => a.productID.Equals(productID)).FirstOrDefault();
            if (dbProduct == null) { return Ok(); }
            db.Products.Remove(dbProduct);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("EditProduct/{productID}")]
        public IActionResult EditProduct(Guid productID,[FromBody] EditProductDto productDto)
        {
            Product dbProduct = db.Products.Where(p => p.productID == productID).FirstOrDefault();
            if (dbProduct == null) return NotFound();
            Product productNewInfo = _mapper.Map<Product>(productDto);

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

        [HttpGet("ListProducts")]
        public IActionResult ListProducts()
        {
            List<ReadProductDto> readProductsDtos = _mapper.Map<List<ReadProductDto>>(db.Products.ToList());
            return Json(readProductsDtos);
        }


        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            return Ok(db.Products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = db.Products.FirstOrDefault(p => p.productID.Equals(id));
            if (product == null) { return NotFound(); }
            return Ok(product);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
