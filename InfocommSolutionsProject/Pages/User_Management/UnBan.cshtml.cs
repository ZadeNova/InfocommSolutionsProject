using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
namespace InfocommSolutionsProject.Pages.User_Management
{
    public class UnBanModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public UnBanModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;

        }

        public Accounts account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var userac = await _context.Users.FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (userac == null)
            {
                return NotFound();
            }
            else
            {
                account = userac;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var account1 = await _context.Users.FirstOrDefaultAsync(m => m.Id == id.ToString());

            if (account1 != null)
            {
                account = account1;
                account.AccountStatus = "Activate".ToString();
                _context.Users.Update(account);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
}
