using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;


namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class CheckOutModel : PageModel
    {
        public const string SessionKeyName = "_PaymentInvalid";
        public const string SessionKeyName1 = "_null_cart";
        public List<ShoppingCartItem> TheShoppingCart { get; set; }
        private readonly UserManager<Accounts> _userManager;
        public Accounts AccountModel { get; set; } = default!;
        public double TotalCost { get; set; }
        public string userid { get; set; }
        public SelectList PaymentList { get; set; }
        public PaymentModel PaymentModel1 { get; set; } = default!;
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
      
        public CheckOutModel(InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void PopulatePaymentList(InfocommSolutionsProjectContext _context, object userobj = null)
        {
            var paymentdata = from PaymentModel in _context.Payment where PaymentModel.Accounts.Id==userid select PaymentModel;
            PaymentList = new SelectList(paymentdata, "Id", "CardNumber", userobj);
        }
        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr?.Id;
        }
        [BindProperty]
        public OrdersModel ordersmodel { get; set; } = default!;
        [BindProperty]
        public Accounts Accounts { get; set; } = default!;
        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public async Task OnGet(string returnUrl = null)
        {
            PopulatePaymentList(_context);
            await GetCurrentUserId();
            PaymentModel1= await _context.Payment.FirstOrDefaultAsync(m => m.Accounts.Id == userid);
            AccountModel = await _context.Users.FirstOrDefaultAsync(m => m.Id == userid);
            returnUrl ??= Url.Content("~/");
             TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            // Check if shopping cart have items.
            if (TheShoppingCart.Count == 0)
                {
               
                    HttpContext.Session.SetString(SessionKeyName1, "Yes");
                    Response.Redirect(returnUrl + "CustomerPages/Shop");
                }
                else
                {
                if (PaymentModel1 == null)
                {
                    HttpContext.Session.SetString(SessionKeyName, "Yes");
                    Response.Redirect(returnUrl + "Identity/Account/Manage/PaymentCreate");
                }
                else
                {

                  
                    // If cart has items calculate the total 
                    foreach (var item in TheShoppingCart)
                    {
                        if (item.Product.DiscountStatus == true)
                        {
                            TotalCost += (item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity;
                        }
                        else
                        {
                            TotalCost += item.Product.Price * item.Quantity;
                        }

                    }

                }


               
                }
         
           
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

          
            var datenow = DateTime.Now;
            returnUrl ??= Url.Content("~/");
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            var total_number_of_distint=TheShoppingCart.Distinct().Count();
            
            for (var i = 0; i <total_number_of_distint; i++)
            { 
                var item = TheShoppingCart[i];
                var product = TheShoppingCart[i].Product;
                var quantity = TheShoppingCart[i].Quantity;

              
                if (item.Product.DiscountStatus == true)
                {
                    TotalCost += (item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity;
                    ordersmodel.Id= Guid.NewGuid();
                    ordersmodel.OrderStatus = "Packing";
                    ordersmodel.Address = Request.Form["AccountModel.Address"].ToString();
                    ordersmodel.PostalCode = Convert.ToInt32(Request.Form["AccountModel.PostalCode"].ToString());
                    ordersmodel.Accounts = _context.Users.First(i => i.Id == Request.Form["AccountModel.Id"].ToString());
                    ordersmodel.DateOfOrder = datenow;
                    ordersmodel.Payment = _context.Payment.First(i => i.Id == Convert.ToInt32(Request.Form["PaymentModel1.Id"]));
                    ordersmodel.quantity = quantity;
                    ordersmodel.PriceOfOrder = item.Product.Price - (item.Product.Price * item.Product.Discount / 100);
                    ordersmodel.Product = _context.Products.First(i => i.Id == product.Id);
                    _context.Orders.Add(ordersmodel);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    TotalCost += item.Product.Price * item.Quantity;
                    ordersmodel.Id = Guid.NewGuid();
                    ordersmodel.OrderStatus = "Packing";
                    ordersmodel.Address = Request.Form["AccountModel.Address"];
                    ordersmodel.PostalCode = Convert.ToInt32(Request.Form["AccountModel.PostalCode"]);
                    ordersmodel.Accounts = _context.Users.First(i => i.Id == Request.Form["AccountModel.Id"]);
                    ordersmodel.DateOfOrder = datenow;
                    ordersmodel.Payment = _context.Payment.First(i => i.Id == Request.Form["PaymentModel1.Id"]);
                    ordersmodel.quantity = quantity;
                    ordersmodel.PriceOfOrder = item.Product.Price - (item.Product.Price * item.Product.Discount / 100);
                    ordersmodel.Product = _context.Products.First(i => i.Id == product.Id);
                    _context.Orders.Add(ordersmodel);
                    await _context.SaveChangesAsync();

                }
            }
            
            return Redirect("/CustomerPages/Shop");


        }
    }
}
