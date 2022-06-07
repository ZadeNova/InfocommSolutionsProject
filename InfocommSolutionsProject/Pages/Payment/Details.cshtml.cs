using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Payment
{
    public class DetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public DetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

      public PaymentModel PaymentModel { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Payment == null)
            {
                return NotFound();
            }

            var paymentmodel = await _context.Payment.FirstOrDefaultAsync(m => m.Id == id);
            if (paymentmodel == null)
            {
                return NotFound();
            }
            else 
            {
                PaymentModel = paymentmodel;
            }
            return Page();
        }
    }
}
