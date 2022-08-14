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
        public IList<ProductModel> Products1 { get; set; }
        public IList<OrdersModel> orderDetails { get; set; }
        public ProductModel Product { get; set; } = default!;
        public IList<RatingsModel> ratings { get; set; }
        public List<ShoppingCartItem> TheShoppingCart { get; set; }
        public string userid { get; set; }
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
                ratings = _context.Ratings.Where(i => i.Product.Id == id).ToList();
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
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == newguid);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
                var cate = product.Category;
                Products1 = _context.Products.Where(i => i.Category == cate).Where(i => i.Id != newguid).ToList();
                ratings = _context.Ratings.Where(i => i.Product.Id == newguid).ToList();
                orderDetails = _context.Orders.Where(i => i.Product.Id == newguid).Where(i => i.Accounts.Id == userid).ToList();
                if (orderDetails.Count == 0)
                {
                    checkifuserhavebuy = 0;
                }
                else
                {
                    checkifuserhavebuy = 1;
                }
                if (ratings.Count != 0)
                {
                    foreach (var idk123 in ratings)
                    {
                        totalnum += idk123.rating;
                        totalcount += 1;

                    }

                    averangenum = totalnum / totalcount;
                }
            }

            return Page();

        }

    }
}

