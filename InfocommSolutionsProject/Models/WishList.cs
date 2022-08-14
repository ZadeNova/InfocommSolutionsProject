using System.ComponentModel.DataAnnotations;
namespace InfocommSolutionsProject.Models
{
    public class WishListModel
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public virtual Accounts Accounts { get; set; }
        public virtual ProductModel Product { get; set; }

    }
}
