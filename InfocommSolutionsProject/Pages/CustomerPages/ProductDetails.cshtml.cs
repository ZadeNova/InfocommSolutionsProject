using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Models;
using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Identity;


namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;



        public ProductDetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IList<ProductModel> Products1 { get; set; } = default!;
        public Accounts accounts { get; set; } = default!;
        public IList<OrdersModel> orderDetails { get; set; }
        public ProductModel Product { get; set; } = default!;
        public IList<RatingsModel> ratings { get; set; } = default!;
        public List<ShoppingCartItem> TheShoppingCart { get; set; }
        public string userid { get; set; }
        [BindProperty]
        public WishListModel wish { get; set; } = default!;
        [BindProperty]
        public RatingsModel rating1 { get; set; } = default!;

        [BindProperty]
        public RatingsModel rating2 { get; set; } = default!;

        public int totalnum { get; set; }
        public int totalcount { get; set; }
        public double averangenum { get; set; }
        public int checkifuserhavebuy { get; set; }
        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr.Id;
        }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            await GetCurrentUserId();
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
                var cate = product.Category;
                Products1 = _context.Products.Where(i => i.Category == cate).Where(i=>i.Id !=id).ToList();
                ratings = await _context.Ratings.Where(i => i.Product.Id == id).ToListAsync();
                //accounts = await _context.Users.ToListAsync();

                //ratings = (from P in _context.Ratings join x in _context.Users on P.Accounts.Id equals x.Id where P.Product.Id == id select new RatingsModel { rating = P.rating, Description = P.Description, CreatedOn = P.CreatedOn }).ToList();
                var lol = _context.Users.ToList();
                orderDetails = _context.Orders.Where(i => i.Product.Id == id).Where(i=>i.Accounts.Id==userid).ToList();
                if (orderDetails.Count == 0)
                {
                    checkifuserhavebuy = 0;
                }
                else {
                    checkifuserhavebuy = 1;
                }
                if (ratings.Count!=0) 
                { 
                         foreach (var idk123 in ratings)
                    {
                        totalnum += idk123.rating;
                        totalcount += 1;
                    
                    }
               
                
                    averangenum = totalnum/totalcount;
                }
               

            }
            return Page();
            
        }

        public IActionResult OnPostAddToShoppingCart(string id , int ItemQuantity)
        {
            System.Diagnostics.Debug.WriteLine($"This is the Item Quantity {ItemQuantity} SUSSSS");

            System.Diagnostics.Debug.WriteLine("Activating from ProductDetails Shopping Cart Side");
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
            return Redirect("~/CustomerPages/ProductDetails?id=" + id);

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

        public async Task<IActionResult> OnPostAddRating(string? id) {
          
            if (id == null) {
                return Page();
            }
            await GetCurrentUserAsync();
            var date= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            rating2.Id = new Guid();
            var newguid = Guid.Parse(id);
          
            rating2.CreatedOn = DateTime.Parse(date);
            rating2.Accounts = _context.Users.First(i => i.Id == Request.Form["accountid"].ToString());
            rating2.Product= _context.Products.First(i=>i.Id ==newguid);
            rating2.Description = Request.Form["Comment"].ToString();
            rating2.rating =Convert.ToInt32( Request.Form["Rating"]);
            _context.Ratings.Add(rating2);
            await _context.SaveChangesAsync();
            return Redirect("~/CustomerPages/ProductDetails?id=" + id);

        }
        public async Task<IActionResult> OnPostAddToWishList(string id)
        {
            Guid id12 = Guid.Parse(id);
            await GetCurrentUserAsync();
            var userid123 = Request.Form["accountid"].ToString();
            var checkwish = _context.wishLists.Where(i => i.Product.Id == id12).Where(i => i.Accounts.Id == userid123).Where(i => i.Status == "Waiting").ToList();

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
                return Redirect("~/CustomerPages/ProductDetails?id=" + id);
            }
            else
            {
                return Redirect("/CustomerPages/ViewWishList");
            }


        }

    }
}

