using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace InfocommSolutionsProject.Models
{
    public class Categories
    {
      
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Category For:")]
        public string CategoryFor { get; set; }

        




    }
}
