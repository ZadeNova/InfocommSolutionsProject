using System.ComponentModel.DataAnnotations;
namespace InfocommSolutionsProject.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Discount Status")]
        public bool DiscountStatus { get; set; }
        [Required]
        public int Discount { get; set; }
        
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        public DateTime UpdatedOn { get; set; }


    }
}
