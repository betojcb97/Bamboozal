using AutoMapper;
using Bamboo.Data;
using Bamboo.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Bamboo.Models
{
    public class Cart
    {
        private readonly BambooContext db;

        [Key]
        public Guid cartID { get; set; }

        public Guid? userID { get; set; }

        public decimal total { get; set; }

        public decimal cost { get; set; }

        public decimal tax { get; set; }

        public decimal subtotal { get; set; }

        public decimal discount { get; set; }

        public string productsIdsAndQuantitiesSerialized { get; set; }

        [NotMapped]
        public Dictionary<string, int> productsIdsAndQuantities
        {
            get
            {
                return productsIdsAndQuantitiesSerialized == null
                    ? null
                    : JsonSerializer.Deserialize<Dictionary<string, int>>(productsIdsAndQuantitiesSerialized);
            }
            set
            {
                productsIdsAndQuantitiesSerialized = JsonSerializer.Serialize(value);
            }
        }

        public DateTime? dateOfCreation { get; set; }

        public Cart(BambooContext db)
        {
            this.db = db;
            cartID = Guid.NewGuid();
            dateOfCreation = DateTime.Now;
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

