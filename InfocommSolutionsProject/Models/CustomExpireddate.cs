using System;
using System.ComponentModel.DataAnnotations;
namespace InfocommSolutionsProject.Models
{
    
    public class CustomExpireddate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return DateTime.Now <= dateTime;
        }
    }
}
