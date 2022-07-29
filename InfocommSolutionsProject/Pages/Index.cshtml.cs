using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Data;

using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly ILogger<IndexModel> _logger;
        public IList<ProductModel> Products { get; set; }
        public IndexModel(ILogger<IndexModel> logger, InfocommSolutionsProjectContext context)
        {
            _logger = logger;
            _context = context;

        }
       
        public void OnGet()
        {
            Products = _context.Products.ToList();
            foreach (var item in Products) System.Diagnostics.Debug.WriteLine(item.Name);
        }
    }
}