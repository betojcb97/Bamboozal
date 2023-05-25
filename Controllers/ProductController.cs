using AutoMapper;
using Bamboo.API;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Linq.Dynamic.Core;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        public ProductController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
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
                CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db);
                if (!loggedUser.ownerOfBusinessID.Equals(dbProduct.businessID)) return BadRequest("You must be the owner of the business that sells this product to remove it!");
                db.Products.Remove(dbProduct);
                db.SaveChanges();
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditProduct/{productID}")]
        public async Task<IActionResult> EditProduct(Guid productID,[FromBody] EditProductDto productDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Product dbProduct = db.Products.Where(p => p.productID == productID).FirstOrDefault();
                if (dbProduct == null) return NotFound();
                CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor, db);
                if (!loggedUser.ownerOfBusinessID.Equals(dbProduct.businessID)) return BadRequest("You must be the owner of the business that sells this product to edit it!");
                Product productNewInfo = mapper.Map<Product>(productDto);

                PropertyInfo[] properties = productNewInfo.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(productNewInfo) != null && property.Name != "productID" && decimal.Parse(property.GetValue(productNewInfo).ToString()) != 0)
                    {
                        property.SetValue(dbProduct, property.GetValue(productNewInfo));
                    }
                    if (property.Name == "imageUrl" && property.GetValue(productNewInfo) == null)
                    {
                        GoogleImage googleImage = new GoogleImage();
                        string response = await googleImage.Search(dbProduct.name + "," + dbProduct.description);
                        dbProduct.imageUrl = response;
                    }
                }

                db.Entry(dbProduct).State = EntityState.Modified;
                db.SaveChanges();
                return CreatedAtAction(nameof(dbProduct), dbProduct);
            }
            else { return Unauthorized(); }
        }

        [HttpGet("ListProducts")]
        public IActionResult ListProducts([FromQuery] string? orderBy="name", [FromQuery] string? ascdesc = "ascending")
        {
            string ordering = orderBy + " " + ascdesc;
            List<ReadProductDto> readProductsDtos = db.Products
            .AsQueryable()
            .OrderBy(ordering)
            .ToList()
            .Select(p => mapper.Map<ReadProductDto>(p))
            .ToList();

            return Json(readProductsDtos);
        }


        [HttpGet("ListProductsOfBusiness/{businessID}")]
        public IActionResult ListProductsOfBusiness(Guid businessID, [FromQuery] string? orderBy = "name", [FromQuery] string? ascdesc = "ascending")
        {
            string ordering = orderBy + " " + ascdesc;
            List<ReadProductDto> readProductsDtos = db.Products
            .AsQueryable()
            .Where(p => p.businessID.Equals(businessID))
            .OrderBy(ordering)
            .ToList()
            .Select(p => mapper.Map<ReadProductDto>(p))
            .ToList();
            return Json(readProductsDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
