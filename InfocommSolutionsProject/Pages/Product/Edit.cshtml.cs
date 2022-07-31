using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace InfocommSolutionsProject.Pages.Product
{
    public class EditModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        

        public EditModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
            
        }

        [BindProperty]
        public ProductModel Product { get; set; } = default!;

        public IFormFile? ImageUpload { get; set; }

        public Guid? ProductID { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            
            var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            
            Product = product;
            System.Diagnostics.Debug.WriteLine($"Discount Status: {product.DiscountStatus}");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("execute model state ss");
                return Page();
            }
            //System.Diagnostics.Debug.WriteLine($"Is it empty? {Product.ImagePath}");
            //System.Diagnostics.Debug.WriteLine($"{ImageUpload != null} {Product.ImagePath != null}");



            // Update date for product
            Product.UpdatedOn = DateTime.Now;

            //var TheProduct = _context.Products.FirstOrDefault(x => x.Id == ProductID);
            
            //Product.CreatedOn = Product.CreatedOn;
            if (ImageUpload != null)
            {
                if (Product.ImagePath != null)
                {
                    System.Diagnostics.Debug.WriteLine("Delete image executed");
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Images/ProductImages", Product.ImagePath);
                    System.IO.File.Delete(filePath);
                }
                Product.ImagePath = ProcessUploadedFile();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(Guid id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string ProcessUploadedFile()
        {
            string UniqueFileName = null;
           
            if (ImageUpload != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/ProductImages");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + ImageUpload.FileName;
                string Filepath = Path.Combine(uploadsFolder, UniqueFileName);
                using (var Filestream = new FileStream(Filepath , FileMode.Create))
                {
                    ImageUpload.CopyTo(Filestream);
                }
            }


            return UniqueFileName;
        }
    }
}
