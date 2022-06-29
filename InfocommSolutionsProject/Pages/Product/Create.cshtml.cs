using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace InfocommSolutionsProject.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CreateModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductModel Product { get; set; } = default!;
        
        public IFormFile ImageUpload { get; set; }


        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            System.Diagnostics.Debug.WriteLine(Product.Name);
            
            // Add image path to product
            Product.ImagePath = ProcessUploadedFile();
            // Add Date to product
            Product.UpdatedOn = DateTime.Now;
            
            //System.Diagnostics.Debug.WriteLine(Product.ImagePath + " YOLO");
            //System.Diagnostics.Debug.WriteLine("Above is imagepath line");

            //System.Diagnostics.Debug.WriteLine($"{!ModelState.IsValid} {_context.Products == null} {Product == null}");

           

            if (!ModelState.IsValid || _context.Products == null || Product == null)
            {
                System.Diagnostics.Debug.WriteLine("Not executing!");
                return Page();
            }

            System.Diagnostics.Debug.WriteLine("Created!");
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


        private string ProcessUploadedFile()
        {
            string UniqueFileName = null;

            if (ImageUpload != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/ProductImages");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + ImageUpload.FileName;
                string Filepath = Path.Combine(uploadsFolder, UniqueFileName);
                using (var Filestream = new FileStream(Filepath, FileMode.Create))
                {
                    ImageUpload.CopyTo(Filestream);
                }
            }


            return UniqueFileName;
        }

    }
}
