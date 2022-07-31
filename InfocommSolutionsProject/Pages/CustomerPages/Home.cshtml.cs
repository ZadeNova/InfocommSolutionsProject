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
        public IList<Categories> Products_Category { get; set; }

        public IList<ProductModel> Products { get; set; } 

        public IList<ProductModel> LatestProducts { get; set; }

        public IList<ProductModel> LatestProducts2 { get; set; }

        public IList<ProductModel> TopRatedProducts { get; set; }

        public IList<ProductModel> TopReviewsProduct { get; set; }

        public string path1 { get; set; }
        public HomeModel(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Products = _context.Products.ToList();

            // Get Latest Products



            //
            //get current path to use for _Aftercustomerloginlayout
            path1 = HttpContext.Request.Path;
            System.Diagnostics.Debug.WriteLine(path1);
            var TheLatestProductList = Products.OrderByDescending(x => x.CreatedOn).Take(6).ToList();
            LatestProducts = TheLatestProductList.Take(3).ToList();
            LatestProducts2 = TheLatestProductList.TakeLast(3).ToList();
            
            System.Diagnostics.Debug.WriteLine(LatestProducts2.LongCount());
            Products_Category = (from cat in _context.Categories where (cat.CategoryFor.ToLower() != "supplier") && (cat.CategoryFor.ToLower() != "suppliers") select cat).ToList();
            foreach (var item in LatestProducts) System.Diagnostics.Debug.WriteLine($"{item.Name} {item.CreatedOn} Wtf bruh {item.UpdatedOn}");
            foreach (var item in LatestProducts2) System.Diagnostics.Debug.WriteLine($"{item.Name} {item.CreatedOn} Wtf bruh {item.UpdatedOn}");

            System.Diagnostics.Debug.WriteLine("Test");
            

        }
    }
}
