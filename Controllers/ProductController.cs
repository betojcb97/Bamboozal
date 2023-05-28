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
using Bamboo.Services;
using Org.BouncyCastle.Utilities.Net;

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
        private LogService logManager;
        public ProductController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.logManager = logManager;
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
                logManager.AddLog($"Product: {product.name} id=({product.productID}) added successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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
                logManager.AddLog($"Product: {dbProduct.name} id=({dbProduct.productID}) removed successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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
                    if (property.GetValue(productNewInfo) != null && property.Name != "productID")
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
                logManager.AddLog($"Product: {dbProduct.name} id=({dbProduct.productID}) edited successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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

        [HttpGet("ListProductInfo/{productID}")]
        public IActionResult ListProductInfo(string productID)
        {
            Product dbProduct = db.Products.Where(p => p.productID.ToString().Equals(productID)).FirstOrDefault();
            if (dbProduct == null) return NotFound();
            ReadProductDto dbProductDto = mapper.Map< ReadProductDto > (dbProduct);
            return Json(dbProduct);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("BusinessProducts")]
        public IActionResult BusinessProducts()
        {
            return View();
        }

    }
}
