using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.Extensions.Configuration;

namespace InfocommSolutionsProject.Pages.Product
{
    
    public class IndexModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string CurrentFilter { get; set; }

        //public IList<ProductModel> Product { get;set; } = default!;

        public PaginatedList<ProductModel> Product { get; set; }



        public async Task OnGetAsync(string searchString , int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<ProductModel> ProductQueryable = from P in _context.Products select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductQueryable = ProductQueryable.Where(s => s.Name.Contains(searchString) || s.Category.Contains(searchString) || s.Description.Contains(searchString));
            }

            if (_context.Products != null)
            {
                //Product = await _context.Products.ToListAsync();
                var pageSize = Configuration.GetValue("PageSize", 4);
                Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

            }

            

        }
    }
}
