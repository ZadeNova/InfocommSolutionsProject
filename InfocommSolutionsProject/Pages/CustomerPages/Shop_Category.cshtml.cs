using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;
namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class Shop_Category : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public Shop_Category(InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ProductModel> Products { get; set; }

        public IList<ProductModel> ProductsDiscount { get; set; }
        public ProductModel Product { get; set; } = default!;
        public IList<ProductModel> LatestProducts { get; set; }

        public IList<ProductModel> LatestProducts2 { get; set; }

        public List<ShoppingCartItem> TheShoppingCart { get; set; }

        public IList<Categories> Products_Category { get; set; }
        public string cate { get; set; }
        private readonly UserManager<Accounts> _userManager;
        [BindProperty]
        public WishListModel wish { get; set; } = default!;
      
        public string userid { get; set; }
        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr?.Id;
        }

        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public async Task <IActionResult> OnGet(string? category )
        {
            if (User.Identity.IsAuthenticated) {
               await GetCurrentUserId();
            }

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
            cate = product.Category;
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
            return Redirect("/CustomerPages/Shop_Category?category="+cate);

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
        public async Task<IActionResult> OnPostAddToWishList(string id)
        {
            Guid id12 = Guid.Parse(id);
            await GetCurrentUserAsync();
            var userid123 = Request.Form["accountid"].ToString();
            var checkwish = _context.wishLists.Where(i => i.Product.Id == id12).Where(i => i.Accounts.Id == userid123).Where(i => i.Status == "Waiting").ToList();
            var product = _context.Products.FirstOrDefault(m => m.Id == id12);
            Product = product;
            cate = product.Category;
            if (checkwish.Count() == 0)
            {
                var datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                wish.Product = _context.Products.First(m => m.Id == id12);
                wish.Accounts = _context.Users.First(m => m.Id == userid123);
                wish.Id = new Guid();
                wish.CreatedOn = DateTime.Parse(datenow);
                wish.Status = "Waiting";
                _context.wishLists.Add(wish);
                await _context.SaveChangesAsync();
                //Products = _context.Products.ToList();
                ////get current path to use for _Aftercustomerloginlayout
                //path1 = HttpContext.Request.Path;
                //System.Diagnostics.Debug.WriteLine(path1);
                //var TheLatestProductList = Products.OrderByDescending(x => x.CreatedOn).Take(6).ToList();
                //LatestProducts = TheLatestProductList.Take(3).ToList();
                //LatestProducts2 = TheLatestProductList.TakeLast(3).ToList();
                //System.Diagnostics.Debug.WriteLine(LatestProducts2.LongCount());
                //Products_Category = (from cat in _context.Categories where (cat.CategoryFor.ToLower() != "supplier") && (cat.CategoryFor.ToLower() != "suppliers") select cat).ToList();
                return Redirect("/CustomerPages/Shop_Category?category=" + cate);
            }
            else
            {
                return Redirect("/CustomerPages/ViewWishList");
            }


        }
    }
}
