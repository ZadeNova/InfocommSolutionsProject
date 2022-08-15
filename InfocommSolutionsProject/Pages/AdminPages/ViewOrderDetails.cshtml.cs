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
namespace InfocommSolutionsProject.Pages.AdminPages
{
    public class ViewOrderDetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public ViewOrderDetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [BindProperty]
        public OrdersModel ordersmodel { get; set; } = default!;
        public double totalamount { get; set; }
        public double findalamount { get; set; }
        public IList<OrdersModel> OrderDetail { get; set; } = default!;
        public IList<ProductModel> Products { get; set; } = default!;
        public IList<PaymentModel> payments { get; set; } = default!;
        public string userid { get; set; }
        //public const string SessionKeyName3 = "_datetiemnow";
        public string datetimenow { get; set; }
        public async Task<IActionResult> OnGetAsync(DateTime date,string id)
        {
            if ( _context.Payment == null)
            {
                return NotFound();
            }
            datetimenow = date.ToString("yyyy-MM-dd HH:mm:ss");
            //HttpContext.Session.SetString(SessionKeyName3, dateTime.ToString("yyyy-MM-dd HH:mm:ss.SSS"));
           
            System.Diagnostics.Debug.WriteLine(date + "dadadadadadada");
            // get the userid from the above function and get the user payment information out only 
          
            OrderDetail = await _context.Orders.Include(i=>i.Accounts).Where(i => i.DateOfOrder.Equals(date) &&i.Accounts.Id==id).ToListAsync();
            
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
        public async Task<IActionResult> OnPostUPDATE(DateTime date , string id)
        {
            //datetimenow = date.ToString("yyyy-MM-dd HH:mm:ss:ss");
            var idk123 = date.ToString("yyyy-MM-dd HH:mm:ss:ss");

            datetimenow = idk123;
            if (id == null)
            {
                return Page();
            }
            var idk  =  _context.Orders.Where(i => i.DateOfOrder==date &&i.Accounts.Id==id).ToList();
            idk.ForEach(i => i.OrderStatus = "Shipping");
            await _context.SaveChangesAsync();
            return Redirect("~/AdminPages/ViewAllOrders"); 
        }

    }
}
