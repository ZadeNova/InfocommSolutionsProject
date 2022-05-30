using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InfocommSolutionsProject.Models
{
    public class OrdersModel
    {
        [Key]
        public int OrderId { get; set; }
        
        public virtual Accounts Accounts { get; set; }
        
        public virtual ProductModel Product { get; set; }
        [Required]
        
        public double PriceOfOrder { get; set; }
        
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateOfOrder { get; set; }
        [Required]
        public string OrderStatus { get; set; }
    }
}
