using AutoMapper;
using Bamboo.Data;
using Bamboo.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bamboo.Models
{
    public class Order
    {
        private TokenValidator tokenValidator;
        private BambooContext db;
        private IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        private LogService logManager;
        public Order(BambooContext db, IMapper mapper, TokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, LogService logManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.tokenValidator = tokenValidator;
            this.httpContextAccessor = httpContextAccessor;
            this.logManager = logManager;
        }

        [Key]
        public Guid orderID { get; set; }

        public decimal total { get; set; }

        public decimal cost { get; set; }

        public decimal tax { get; set; }

        public decimal subtotal { get; set; }

        public decimal discount { get; set; }

        [Required(ErrorMessage = "The userID of the Order must be provided")]
        public Guid? userID { get; set; }

        public virtual CustomUser user { get; set; }

        [NotMapped]
        public Dictionary<string, int> productsIdsAndQuantities { get; set; }

        public Guid? deliveryAddressID { get; set; }

        public virtual Address deliveryAddress { get; set; }

        public Order()
        {
            orderID = Guid.NewGuid();
        }

        public void CalculateSums()
        {
            decimal subtotalSum = 0;
            decimal costSum = 0;
            decimal taxSum = 0;
            decimal discountSum = 0;

            foreach (var item in productsIdsAndQuantities)
            {
                Product product = db.Products.FirstOrDefault(p => p.productID.ToString().Equals(item.Key));
                if (product != null)
                {
                    decimal price = product.price;
                    decimal tax = product.tax;
                    decimal cost = product.cost;
                    decimal discount = product.discount;
                    int quantity = item.Value;
                    subtotalSum += price * quantity;
                    discountSum += discount * quantity;
                    taxSum += tax * quantity;
                    costSum += cost * quantity;
                }
            }

            total = subtotalSum - discountSum;
            subtotal = subtotalSum;
            cost = costSum;
            tax = taxSum;
            discount = discountSum;
        }
    }
}
