using AutoMapper;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Bamboo.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Net;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        private LogService logManager;
        public CartController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.logManager = logManager;
        }

        [HttpPost("AddCart")]
        public IActionResult AddCart([FromBody] AddCartDto cartDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            CustomUser loggedUser = Util.Util.getLoggedUser(httpContextAccessor,db);
            if (authorized)
            {
                Cart cart = new Cart(db)
                {
                    userID = loggedUser.userID,
                    productsIdsAndQuantities = cartDto.productsIdsAndQuantities,
                };
                cart.CalculateSums();

                db.Carts.Add(cart);
                db.SaveChanges();
                logManager.AddLog($"Cart: id=({cart.cartID}) added successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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
                logManager.AddLog($"Cart: id=({dbCart.cartID}) removed successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
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

                dbCart.productsIdsAndQuantities = CartNewInfo.productsIdsAndQuantities;

                PropertyInfo[] properties = CartNewInfo.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(CartNewInfo) != null && property.Name != "CartID" && property.Name != "ProductsIdsAndQuantitiesList")
                    {
                        property.SetValue(dbCart, property.GetValue(CartNewInfo));
                    }
                }

                db.Entry(dbCart).State = EntityState.Modified;
                db.SaveChanges();
                logManager.AddLog($"Cart: id=({dbCart.cartID}) edited successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return CreatedAtAction(nameof(dbCart), dbCart);
            }
            else { return Unauthorized(); }
        }

        [HttpGet("ListCarts")]
        public IActionResult ListCarts()
        {
            CustomUser dbUser = Util.Util.getLoggedUser(httpContextAccessor, db);
            if (dbUser == null) return Json(null);
            List<Cart> carts = db.Carts.Where(c => c.userID.Equals(dbUser.userID)).ToList();

            List<ReadCartDto> readCartDtos = new List<ReadCartDto>();

            foreach (var cart in carts)
            {
                cart.CalculateSums();

                ReadCartDto cartDto = new ReadCartDto
                {
                    cartID = cart.cartID,
                    userID = cart.userID,
                    productsIdsAndQuantities = cart.productsIdsAndQuantities,
                    dateOfCreation = cart.dateOfCreation,
                    total = cart.total,
                    subtotal = cart.subtotal,
                    tax = cart.tax,
                    discount = cart.discount
                };

                readCartDtos.Add(cartDto);
            }

            dynamic response = new ExpandoObject();
            response.cartsDto = readCartDtos;
            List<ReadProductDto> productsDto = new List<ReadProductDto>();
            foreach (var cart in readCartDtos)
            {
                foreach (var productQuantities in cart.productsIdsAndQuantities)
                {
                    foreach (var productId in productQuantities.Keys)
                    {
                        productsDto.Add(mapper.Map<ReadProductDto>(db.Products.Where(p => p.productID.ToString().Equals(productId)).FirstOrDefault()));
                    }
                }
            }
            response.products = productsDto;
            return Json(response);
        }


        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
