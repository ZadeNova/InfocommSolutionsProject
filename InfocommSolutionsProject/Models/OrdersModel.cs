using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InfocommSolutionsProject.Models
{
    public class OrdersModel
    {
        [Key]
        public int OrderId { get; set; }
        
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
        [RegularExpression(@"(^[0-9])", ErrorMessage = "Digit only")]
        [Required]
        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        public virtual Accounts Accounts { get; set; }

        public virtual ProductModel Product { get; set; }

        public virtual PaymentModel Payment { get; set; }

       
    }
}

