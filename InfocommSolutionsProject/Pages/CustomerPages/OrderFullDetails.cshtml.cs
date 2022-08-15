using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;
namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class OrderFullDetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public OrderFullDetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public double totalamount { get; set; }
        public double findalamount { get; set; }
        public IList<OrdersModel> OrderDetail { get; set; } = default!;
        public IList<ProductModel> Products { get; set; } = default!;
        public IList<PaymentModel> payments { get; set; } = default!;
        public string userid { get; set; }
        //public const string SessionKeyName3 = "_datetiemnow";
        public string datetimenow { get; set; }
        public async Task<IActionResult> OnGetAsync(DateTime date)
        {
            if ( _context.Payment == null)
            {
                return NotFound();
            }
            datetimenow = date.ToString("yyyy-MM-dd HH:mm:ss");
            //HttpContext.Session.SetString(SessionKeyName3, dateTime.ToString("yyyy-MM-dd HH:mm:ss.SSS"));
            await GetCurrentUserId();
            System.Diagnostics.Debug.WriteLine(date + "dadadadadadada");
            // get the userid from the above function and get the user payment information out only 
          
            OrderDetail = await _context.Orders.Where(i => i.Accounts.Id == userid && i.DateOfOrder.Equals(date)).ToListAsync();
            
            Products = _context.Products.ToList();

            payments = _context.Payment.ToList();
            foreach (var item in OrderDetail) {
                totalamount += item.PriceOfOrder;
            }
            if (totalamount >= 99)
            {
                findalamount += totalamount;
              
            }
            else {
                findalamount += totalamount;
                findalamount += 9.9;
            }

            return Page();
        }
        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr?.Id;
        }
        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
