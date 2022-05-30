using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfocommSolutionsProject.Models
{
    public class SupplierModel
    {
        [Required]
        [Key]
        public Guid SupplierId { get; set; }
        [Required]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }
        [Required]
        [Display(Name = "Supplier Category")]
        public string SupplierCategory { get; set; }

    }
}
