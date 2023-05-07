using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("AddProductForm")]
        public IActionResult AddProductForm([FromBody] AddProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            Product exists = db.Products.Where(p => p.name == product.name).FirstOrDefault();
            if (exists != null) { return StatusCode(StatusCodes.Status406NotAcceptable); }
            db.Products.Add(product);
            db.SaveChanges();
            return CreatedAtAction(nameof(product), product);
        }

        [HttpPost("EditProduct")]
        public IActionResult EditProduct([FromBody] EditProductDto productDto)
        {
            Product dbProduct = db.Products.Where(p => p.productID == productDto.productId).FirstOrDefault();
            if (dbProduct == null) return NotFound();
            Product productNewInfo = _mapper.Map<Product>(productDto);

            dbProduct.name = productNewInfo.name ?? productNewInfo.name;
            dbProduct.description = productNewInfo.description ?? productNewInfo.description;
            dbProduct.price = productNewInfo.price > 0 ? productNewInfo.price : dbProduct.price;
            dbProduct.imageUrl = productNewInfo.imageUrl ?? productNewInfo.imageUrl;
            dbProduct.businessID = productNewInfo.businessID ?? productNewInfo.businessID;

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
