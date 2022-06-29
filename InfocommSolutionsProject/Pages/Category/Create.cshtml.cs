using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Category
{
    public class CreateModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public CreateModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Categories Categories { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Categories == null || Categories == null)
            {
                return Page();
            }

            _context.Categories.Add(Categories);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
