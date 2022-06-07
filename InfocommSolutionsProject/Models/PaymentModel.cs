using System.ComponentModel.DataAnnotations;

namespace InfocommSolutionsProject.Models
{
    public class PaymentModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Card Number")]
        public int CardNumber { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Date of Expiry")]
        public DateTime DateOfExpiry { get; set; }
        [Required]
        [Display(Name = "Security Code")]
        public int SecurityCode { get; set; }
        [Required]
        public virtual Accounts Accounts { get; set; }
    }
}
