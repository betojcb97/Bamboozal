using AutoMapper;
using Bamboo.API;
using Bamboo.Data;
using Bamboo.DTO;
using Bamboo.Models;
using Bamboo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Net;
using System.Net;
using System.Reflection;

namespace Bamboo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {

        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        private LogService logManager;
        public OrderController(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.logManager = logManager;
        }

        [HttpPost("AddOrder/{cartID}")]
        public async Task<IActionResult> AddOrder(Guid cartID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Cart cart = db.Carts.Where(c => c.cartID.Equals(cartID)).FirstOrDefault();
                Order order = new Order
                {
                    userID = cart.userID,
                    products = new List<Product>(cart.products),
                    total = cart.products.Sum(p => p.price - p.discount), 
                    subtotal = cart.products.Sum(p => p.price),
                    discount = cart.products.Sum(p => p.discount),
                    cost = cart.products.Sum(p => p.cost),
                    businessID = cart.products.First().businessID,
                    
                };
                db.Orders.Add(order);
                db.Carts.Remove(cart);
                db.SaveChanges();
                logManager.AddLog($"Order: id=({order.orderID}) added successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return CreatedAtAction(nameof(order), order);
            }
            else { return Unauthorized(); }
        }

        [HttpPost("RemoveOrder/{OrderID}")]
        public IActionResult RemoveOrder(Guid OrderID)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Order dbOrder = db.Orders.Where(a => a.orderID.Equals(OrderID)).FirstOrDefault();
                if (dbOrder == null) { return Ok(); }
                dbOrder.products.Clear();
                db.Orders.Remove(dbOrder);
                db.SaveChanges();
                logManager.AddLog($"Order: id=({dbOrder.orderID}) removed successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return Ok();
            }
            else { return Unauthorized(); }
        }

        [HttpPost("EditOrder/{orderID}")]
        public IActionResult EditOrder(Guid orderID, [FromBody] EditOrderDto OrderDto)
        {
            bool authorized = tokenValidator.ValidateToken();
            if (authorized)
            {
                Order dbOrder = db.Orders.Where(p => p.orderID == orderID).FirstOrDefault();
                if (dbOrder == null) return NotFound();
                Order OrderNewInfo = mapper.Map<Order>(OrderDto);

                PropertyInfo[] properties = OrderNewInfo.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(OrderNewInfo) != null && property.Name != "OrderID")
                    {
                        property.SetValue(dbOrder, property.GetValue(OrderNewInfo));
                    }
                }

                db.Entry(dbOrder).State = EntityState.Modified;
                db.SaveChanges();
                logManager.AddLog($"Order: id=({dbOrder.orderID}) edited successfully by: {Util.Util.getLoggedUser(httpContextAccessor, db).userFirstName} id=({Util.Util.getLoggedUser(httpContextAccessor, db).userID})!");
                return CreatedAtAction(nameof(dbOrder), dbOrder);
            }
            else { return Unauthorized(); }
        }

        [HttpGet("ListOrders")]
        public IActionResult ListOrders()
        {
            CustomUser dbUser = Util.Util.getLoggedUser(httpContextAccessor, db);
            List<ReadOrderDto> readOrdersDtos = mapper.Map<List<ReadOrderDto>>(db.Orders.ToList());
            if (dbUser != null) { readOrdersDtos = readOrdersDtos.Where(u => u.userID.Equals(dbUser.userID)).ToList(); }
            return Json(readOrdersDtos);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
