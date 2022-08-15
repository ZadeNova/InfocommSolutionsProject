using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace InfocommSolutionsProject.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DeleteModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IWebHostEnvironment webHostENVironment)
        {
            _context = context;
            this.webHostEnvironment = webHostENVironment;
        }

        [BindProperty]
      public ProductModel Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            System.Diagnostics.Debug.WriteLine(product.ImagePath + " HHHHHHHHHHHHHHHHHHH");
            string test = Path.Combine(webHostEnvironment.WebRootPath, "Images/ProductImages", product.ImagePath);
            System.Diagnostics.Debug.WriteLine(test);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                if (product.ImagePath != null)
                {
                    System.Diagnostics.Debug.WriteLine("Delete image executed");
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/ProductImages", product.ImagePath);
                    System.IO.File.Delete(filePath);

                }

                Product = product;
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();

                // Delete Image related to product also.
                
               

            }

            return RedirectToPage("./Index");
        }
    }
}
