using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Supplier
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
            PopulateSupplierCategoryList(_context);
            return Page();
        }
      
        public SelectList SupplierCategorylist { get; set; }
        public void PopulateSupplierCategoryList(InfocommSolutionsProjectContext _context, object userobj = null)
        {
            var suplierca1 = from Categories in _context.Categories select Categories;
            SupplierCategorylist = new SelectList(suplierca1, "CategoryId", "CategoryFor", userobj);
        }

        [BindProperty]
        public SupplierModel SupplierModel { get; set; } = default!;
        public string sid { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            sid = Request.Form["SupplierCategory1"].ToString();
           // SupplierModel.CategoryId = _context.Categories.First(i => i.CategoryFor.ToString() == sid);
            //if (!ModelState.IsValid || _context.Suppliers == null || SupplierModel == null)
            //  {
            //      return Page();
            //  }

            _context.Suppliers.Add(SupplierModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
