﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InfocommSolutionsProject.Models
{
    public class OrdersModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Price of Order")]
        public double PriceOfOrder { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        [Display(Name = "Date of Order")]
        public DateTime DateOfOrder { get; set; }
        [Required]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [RegularExpression(@"(^[0-9]{6}$)", ErrorMessage = "Only 6 digit is allow")]
        [Required]
        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        [Required]
        [Display(Name = "Free Shipping")]
        public int FreeShipping { get; set; }


        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        public virtual Accounts Accounts { get; set; }

        public virtual ProductModel Product { get; set; }

        public virtual PaymentModel Payment { get; set; }
    }
}

