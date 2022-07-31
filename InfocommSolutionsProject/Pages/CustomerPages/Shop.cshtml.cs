using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class ShopModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public ShopModel(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public IList<ProductModel> Products { get; set; }

        public IList<ProductModel> ProductsDiscount { get; set; }

        public IList<ProductModel> LatestProducts { get; set; }

        public IList<ProductModel> LatestProducts2 { get; set; }


        public void OnGet()
        {
            Products = _context.Products.ToList();
            ProductsDiscount = (from prod in _context.Products where prod.DiscountStatus == true select prod).OrderByDescending(x => x.Discount).Take(6).ToList();

            var TheLatestProductList = Products.OrderByDescending(x => x.CreatedOn).Take(6).ToList();
            LatestProducts = TheLatestProductList.Take(3).ToList();
            LatestProducts2 = TheLatestProductList.TakeLast(3).ToList();

            foreach (var item in ProductsDiscount) System.Diagnostics.Debug.WriteLine($"{item.Name} {item.DiscountStatus} {item.Discount} {item.Price - (item.Price * item.Discount/100)}");





        }
    }
}
