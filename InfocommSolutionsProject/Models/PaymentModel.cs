using System.ComponentModel.DataAnnotations;
using System;


namespace InfocommSolutionsProject.Models
{
    public class PaymentModel
    {
        [Key]
        public int Id { get; set; }

       
        [RegularExpression(@"(^4[0-9]{12}(?:[0-9]{3})?$)|(^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$)|(3[47][0-9]{13})|(^3(?:0[0-5]|[68][0-9])[0-9]{11}$)|(^6(?:011|5[0-9]{2})[0-9]{12}$)|(^(?:2131|1800|35\d{3})\d{11}$)", ErrorMessage = "Invalid Credit Card Format")]
        [Required]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }
        [DataType(DataType.Date)]


       
        [Required]
        //[CustomExpireddate(ErrorMessage = "Expired Date must be bigger then today date")]

        [Display(Name = "Date of Expiry")]
        public DateTime DateOfExpiry { get; set; }
       
        [RegularExpression(@"^\d{3}$",ErrorMessage ="Security Code consist of 3 digit !")]
        [Required]
        [Display(Name = "Security Code")]
        public int SecurityCode { get; set; } 
        [Required]
        public virtual Accounts? Accounts { get; set; }
    }


}
