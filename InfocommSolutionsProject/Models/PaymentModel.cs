using System.ComponentModel.DataAnnotations;

namespace InfocommSolutionsProject.Models
{
    public class PaymentModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CardNumber { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateOfExpiry { get; set; }
        [Required]
        public int SecurityCode { get; set; }
        [Required]
        public virtual Accounts Accounts { get; set; }
    }
}
