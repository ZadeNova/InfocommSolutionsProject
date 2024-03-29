﻿using System;
using System.ComponentModel.DataAnnotations;

namespace InfocommSolutionsProject.Models
{
    public class SupplierOrdersModel
    {
        [Key]
        [Required]
        public Guid SupplierOrderId { get; set; }
        [Required]
        public virtual SupplierModel SupplierID { get; set; }
        [Required]
        public virtual Accounts AccountID { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfOrder { get; set; }
        [Required]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        [Required]
        [Display(Name = "Order Price")]
        public double OrderPrice { get; set; }





    }
}
