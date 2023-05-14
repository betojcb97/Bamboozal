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

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        public CartController(BambooContext context, IMapper _mapper, TokenValidator _tokenValidator)
        {
            db = context;
            mapper = _mapper;
            tokenValidator = _tokenValidator;
        }

        [HttpPost("AddCart")]
        public IActionResult AddCart([FromBody] AddCartDto cartDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            CustomUser loggedUser = tokenValidator.getLoggedUser();
            if (authorized)
            {
                Cart cart = new Cart
                {
                    userID = loggedUser.userID,
                    productsIds = cartDto.productsIds,
                    products = new List<Product>(),
                };

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
            else { return Unauthorized(); }
        }


        [HttpPost("RemoveCart/{cartID}")]
        public IActionResult RemoveCart(Guid cartID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Cart dbCart = db.Carts.Where(a => a.cartID.Equals(cartID)).FirstOrDefault();
                if (dbCart == null) { return Ok(); }
                db.Carts.Remove(dbCart);
                db.SaveChanges();
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditCart/{cartID}")]
        public IActionResult EditCart(Guid cartID, [FromBody] EditCartDto cartDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Cart dbCart = db.Carts.Where(a => a.cartID.Equals(cartID)).FirstOrDefault();
                if (dbCart == null) return NotFound();
                Cart CartNewInfo = mapper.Map<Cart>(cartDto);

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
            else { return Unauthorized(); }
        }

        [HttpGet("ListCarts")]
        public IActionResult ListCarts()
        {
            List<ReadCartDto> readCartDtos = mapper.Map<List<ReadCartDto>>(db.Carts.ToList());
            return Json(readCartDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
