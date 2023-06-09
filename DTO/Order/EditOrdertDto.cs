﻿using Bamboo.Models;
using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class EditOrderDto
    {
        public Guid? OrderID { get; set; }

        public decimal? total { get; set; }
                      
        public decimal? cost { get; set; }
                      
        public decimal? tax { get; set; }
                      
        public decimal? subtotal { get; set; }
                      
        public decimal? discount { get; set; }

        public Guid? userID { get; set; }

        public Guid? businessID { get; set; }

        public Dictionary<Guid, int> productsIdsAndQuantities { get; set; }

        public Guid? deliveryAddressID { get; set; }
    }
}
