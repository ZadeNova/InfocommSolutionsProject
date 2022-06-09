using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Areas.Identity.Pages.Account.Manage
{
    public class DeleteModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public DeleteModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Payment == null)
            {
                return NotFound();
            }
            var paymentmodel = await _context.Payment.FindAsync(id);

            if (paymentmodel != null)
            {
                PaymentModel = paymentmodel;
                _context.Payment.Remove(PaymentModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./PaymentIndex");
        }
    }
}
