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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
        public IList<PaymentModel> PaymentModel2 { get; set; } = default!;
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public CheckOutModel(InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public void PopulatePaymentList(InfocommSolutionsProjectContext _context, object userobj = null)
        {
            var paymentdata = from PaymentModel in _context.Payment where PaymentModel.Accounts.Id == userid select PaymentModel;
            PaymentList = new SelectList(paymentdata, "Id", "CardNumber", userobj);
        }
        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr.Id;
        }
        public const string SessionKeyName2 = "_cardid";

        [BindProperty]
        public OrdersModel ordersmodel { get; set; } = default!;
        [BindProperty]
        public ProductModel Product { get; set; } = default!;
        [BindProperty]
        public Accounts Accounts { get; set; } = default!;
        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public int cardid { get; set; }
        public async Task OnGet(string returnUrl = null)
        {
            
         
            PopulatePaymentList(_context);
            await GetCurrentUserId();
            PaymentModel1 = await _context.Payment.FirstOrDefaultAsync(m => m.Accounts.Id == userid);
            PaymentModel2 = await _context.Payment.Where(i => i.Accounts.Id == userid).ToListAsync();
            AccountModel = await _context.Users.FirstOrDefaultAsync(m => m.Id == userid);
            if (cardid == 0)
            {
                cardid = PaymentModel1.Id;
            }
            returnUrl ??= Url.Content("~/");
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            // Check if shopping cart have items.

            if (TheShoppingCart == null)
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
                            TotalCost +=Math.Round((item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity,2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            TotalCost +=Math.Round( item.Product.Price * item.Quantity,2, MidpointRounding.AwayFromZero);
                        }

                    }

                }



            }


        }



        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
           
            var address = Request.Form["AccountModel.Address"];
            var phn = Request.Form["AccountModel.PhoneNumber"];
            var pos = Request.Form["AccountModel.PostalCode"];
            var newadd = Request.Form["ordersmodel.Address"];
            var newzip = Request.Form["ordersmodel.PostalCode"];
            
            var account = _context.Users.First(i => i.Id == Request.Form["AccountModel.Id"].ToString());
            if (account.Address == null || account.PostalCode == null || account.PhoneNumber == null)
            {
                Accounts = account;
                Accounts.Address = address.ToString();
                Accounts.PostalCode = pos.ToString();
                Accounts.PhoneNumber = phn.ToString();
                _context.Attach(Accounts).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else if (account.Address != address.ToString() || account.PhoneNumber != phn.ToString() || account.PostalCode != pos.ToString())
            {
                Accounts = account;
                Accounts.Address = address.ToString();
                Accounts.PostalCode = pos.ToString();
                Accounts.PhoneNumber = phn.ToString();
                _context.Attach(Accounts).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            returnUrl ??= Url.Content("~/");
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            var total_number_of_distint = TheShoppingCart.Distinct().Count();
            var cart = TheShoppingCart;

            for (var i = 0; i < total_number_of_distint; i++)
            {

                var item = TheShoppingCart[i];
                var product = TheShoppingCart[i].Product;
                var quantity = TheShoppingCart[i].Quantity;

                System.Diagnostics.Debug.WriteLine($"{Request.Form["AccountModel.Id"]} SUSSYBAKA!!!");
                if (item.Product.DiscountStatus == true)
                {
                    TotalCost += (item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity;
                }
                else {
                    TotalCost += item.Product.Price * item.Quantity;
                }
               
                ordersmodel.Id = Guid.NewGuid();
                ordersmodel.OrderStatus = "Packing";
                if (!string.IsNullOrEmpty(newadd))
                {
                    ordersmodel.Address = newadd.ToString();
                }
                else
                {
                    ordersmodel.Address = address.ToString();
                }
                if (!string.IsNullOrEmpty(newzip))
                {
                    ordersmodel.PostalCode = int.Parse(newzip);
                }
                else
                {
                    ordersmodel.PostalCode = int.Parse(pos);
                }

                ordersmodel.Accounts = _context.Users.First(i => i.Id == Request.Form["AccountModel.Id"].ToString());
                ordersmodel.DateOfOrder = DateTime.Parse(datenow);
                ordersmodel.Payment = _context.Payment.First(i => i.Id == Convert.ToInt32(Request.Form["PaymentModel1.CardNumber"]));

                ordersmodel.quantity = quantity;
                ordersmodel.PriceOfOrder =Math.Round ((item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity,2, MidpointRounding.AwayFromZero);
                ordersmodel.Product = _context.Products.First(i => i.Id == product.Id);

                _context.Orders.Add(ordersmodel);
                int changes = await _context.SaveChangesAsync();
                if (changes > 0)
                {
                    var product1 = await _context.Products.FirstOrDefaultAsync(m => m.Id == product.Id);
                    if (product1 != null)
                    {
                        Product = product1;
                        Product.UpdatedOn = DateTime.Parse(datenow);
                        Product.Quantity -= quantity;
                        _context.Attach(Product).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                    }

                }
                
            }
            HttpContext.Session.Remove("ShoppingCart");
            return Redirect("/CustomerPages/Shop");


        }
    }
}
