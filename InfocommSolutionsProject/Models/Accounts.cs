using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Net;

namespace InfocommSolutionsProject.Models
{
    public class Accounts : IdentityUser
    {
        //[Required]
        //public int Id { get; set; }
        //[Required]
        //public string UserName { get; set; }

        [Required]
        public string FirstName     { get; set; }
        [Required]
        public string LastName { get; set; }
        //[Required]
        //public string Password { get; set; }
        
        //public string Role { get; set; }
        //[Required]
        //public string AccountStatus { get; set; }

        public string? Address { get; set; }
        
        public string? PostalCode { get; set; }
        //[Required]
        //public string Email { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        //public int AccessFailedCount { get; set; }

        //public bool TwoFactorEnabled { get; set; }


    }
}
