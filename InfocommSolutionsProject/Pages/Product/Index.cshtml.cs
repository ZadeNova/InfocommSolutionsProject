using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Product
{
    
    public class IndexModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }

        public IList<ProductModel> Product { get;set; } = default!;

        public async Task OnGetAsync(string searchString)
        {
            CurrentFilter = searchString;

            IQueryable<ProductModel> ProductQueryable = from P in _context.Products select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductQueryable = ProductQueryable.Where(s => s.Name.Contains(searchString) || s.Category.Contains(searchString) || s.Description.Contains(searchString));
            }

            if (_context.Products != null)
            {
                //Product = await _context.Products.ToListAsync();
                Product = await ProductQueryable.AsNoTracking().ToListAsync();
            }
        }
    }
}
