using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.CustomerPages
{

    public class HomeModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;


        public IList<ProductModel> Products { get; set; } 


        public HomeModel(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Products = _context.Products.ToList();
            foreach(var item in Products) System.Diagnostics.Debug.WriteLine(item.Name);

        }
    }
}
