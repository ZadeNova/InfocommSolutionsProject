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
        public IList<Categories> Products_Category { get; set; }
        public IndexModel(ILogger<IndexModel> logger, InfocommSolutionsProjectContext context)
        {
            _logger = logger;
            _context = context;

        }
       
        public void OnGet()
        {
            Products = _context.Products.ToList();
            //Products_Category = _context.Categories.Where(x=>x.CategoryName );
            //Products_Category = _context.Categories.ToList();
            Products_Category = (from cat in _context.Categories where (cat.CategoryFor.ToLower() != "supplier") && (cat.CategoryFor.ToLower() != "suppliers") select cat).ToList();
            foreach (var itesm in Products_Category) System.Diagnostics.Debug.WriteLine(itesm.CategoryName);
            foreach (var item in Products) System.Diagnostics.Debug.WriteLine(item.Name);
        }
    }
}