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
    public class CartController : Controller
    {

        private BambooContext db;
        private IMapper _mapper;
        public CartController(BambooContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [HttpPost("AddCart")]
        public IActionResult AddCart([FromBody] AddCartDto cartDto)
        {
            // Create a new Cart
            Cart cart = new Cart
            {
                userID = cartDto.userID,
                productsIds = cartDto.productsIds,
                products = new List<Product>(),
            };

            // Get products from the database based on product IDs
            if (cartDto.productsIds != null)
            {
                foreach (var productId in cartDto.productsIds)
                {
                    var product = db.Products.Find(productId);
                    if (product != null)
                    {
                        cart.products.Add(product);
                    }
                }
            }

            db.Carts.Add(cart);
            db.SaveChanges();

            return CreatedAtAction(nameof(cart), cart);
        }


        [HttpPost("RemoveCart/{cartID}")]
        public IActionResult RemoveCart(Guid cartID)
        {
            Cart dbCart = db.Carts.Where(a => a.cartID.Equals(cartID)).FirstOrDefault();
            if (dbCart == null) { return Ok(); }
            db.Carts.Remove(dbCart);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("EditCart/{cartID}")]
        public IActionResult EditCart(Guid cartID, [FromBody] EditCartDto cartDto)
        {
            Cart dbCart = db.Carts.Where(a => a.cartID.Equals(cartID)).FirstOrDefault();
            if (dbCart == null) return NotFound();
            Cart CartNewInfo = _mapper.Map<Cart>(cartDto);

            PropertyInfo[] properties = CartNewInfo.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(CartNewInfo) != null && property.Name != "CartID")
                {
                    property.SetValue(dbCart, property.GetValue(CartNewInfo));
                }
            }

            db.Entry(dbCart).State = EntityState.Modified;
            db.SaveChanges();
            return CreatedAtAction(nameof(dbCart), dbCart);
        }

        [HttpGet("ListCarts")]
        public IActionResult ListCarts()
        {
            List<ReadCartDto> readCartDtos = _mapper.Map<List<ReadCartDto>>(db.Carts.ToList());
            return Json(readCartDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCartById(Guid id)
        {
            var Cart = db.Carts.FirstOrDefault(a => a.cartID.Equals(id));
            if (Cart == null) { return NotFound(); }
            return Ok(Cart);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
