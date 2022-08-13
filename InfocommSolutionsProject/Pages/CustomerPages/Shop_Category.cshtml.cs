using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class Shop_Category : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public Shop_Category(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public IList<ProductModel> Products { get; set; }

        public IList<ProductModel> ProductsDiscount { get; set; }
        public ProductModel Product { get; set; } = default!;
        public IList<ProductModel> LatestProducts { get; set; }

        public IList<ProductModel> LatestProducts2 { get; set; }

        public List<ShoppingCartItem> TheShoppingCart { get; set; }

        public IList<Categories> Products_Category { get; set; }
        public string cate { get; set; }
        public async Task <IActionResult> OnGet(string? category )
        {
            if (category == null || _context.Products == null)
            {
                return NotFound();
            }
            Products = _context.Products.Where(i=>i.Category==category).ToList();
            cate = category;
            ProductsDiscount = (from prod in _context.Products where prod.DiscountStatus == true where prod.Category==category select prod).OrderByDescending(x => x.Discount).Take(6).ToList();
            Products_Category = (from cat in _context.Categories where (cat.CategoryFor.ToLower() != "supplier") && (cat.CategoryFor.ToLower() != "suppliers") select cat).ToList();
            var TheLatestProductList = Products.OrderByDescending(x => x.CreatedOn).Take(6).ToList();
            LatestProducts = TheLatestProductList.Take(3).ToList();
            LatestProducts2 = TheLatestProductList.TakeLast(3).ToList();
            foreach (var item in ProductsDiscount) System.Diagnostics.Debug.WriteLine($"{item.Name} {item.DiscountStatus} {item.Discount} {item.Price - (item.Price * item.Discount/100)}");
           
            return Page();
        }
        public IActionResult OnPostAddToShoppingCart(string id, int ItemQuantity)
        {
            Guid id1=Guid.Parse(id);
            var product =  _context.Products.FirstOrDefault(m => m.Id == id1);
            Product = product;
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            if (TheShoppingCart == null)
            {
                TheShoppingCart = new List<ShoppingCartItem>();
                TheShoppingCart.Add(
                    new ShoppingCartItem
                    {
                        Product = _context.Products.Find(Guid.Parse(id)),
                        Quantity = ItemQuantity
                    });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);

            }
            else
            {
                int index = Exists(TheShoppingCart, id);
                if (index == -1)
                {
                    TheShoppingCart.Add(new ShoppingCartItem
                    {
                        Product = _context.Products.Find(Guid.Parse(id)),
                        Quantity = ItemQuantity

                    });
                }
                else
                {
                    TheShoppingCart[index].Quantity += ItemQuantity;

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);



            }




            //return RedirectToPage("ProductDetails" + id);
            return Redirect("~/CustomerPages/Shop");

        }


        private int Exists(List<ShoppingCartItem> cart, string id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.ToString() == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
