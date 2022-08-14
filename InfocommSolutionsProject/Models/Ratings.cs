using System.ComponentModel.DataAnnotations;
namespace InfocommSolutionsProject.Models
{
    public class RatingsModel
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public int rating { get; set; }
        
        public DateTime CreatedOn { get; set; }
        public virtual Accounts Accounts { get; set; }
        public virtual ProductModel Product { get; set; }


    }
}
