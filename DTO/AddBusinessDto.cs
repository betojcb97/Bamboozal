﻿using System.ComponentModel.DataAnnotations;

namespace Bamboo.DTO
{
    public class AddBusinessDto
    {
        [Required(ErrorMessage = "The name of the business must be provided")]
        [MaxLength(50, ErrorMessage = "The size of the name must not exceed 50 characters")]
        public string name { get; set; }

        public string description { get; set; }

        [Required(ErrorMessage = "The email of the business must be provided")]
        [MaxLength(70, ErrorMessage = "The size of the email must not exceed 70 characters")]
        public string email { get; set; }

        [Required(ErrorMessage = "The phone of the business must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the phone must not exceed 20 characters")]
        public string phone { get; set; }

        public Guid? addressID { get; set; }

        [Required(ErrorMessage = "The city of the business must be provided")]
        [MaxLength(20, ErrorMessage = "The size of the phone must not exceed 20 characters")]
        public string city { get; set; }

        public bool isActive { get; set; }

        public string? logoUrl { get; set; }

    }
}
