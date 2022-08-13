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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class OrderDetails : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public OrderDetails(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
     
        public IList<OrdersModel> OrderDetail { get;set; } = default!;
        public IList<ProductModel> Products { get; set; } = default!;
        public string userid { get; set; }
        //public IList<PaymentModel> UserSelectList { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Payment != null)
            {
                //get current login user id 
                await GetCurrentUserId();
                // get the userid from the above function and get the user payment information out only 
                OrderDetail = await _context.Orders.Where(i => i.Accounts.Id == userid).ToListAsync();
                Products = _context.Products.ToList();
            
                //OrderDetail = (from cat in _context.Orders where (cat.Accounts.Id==userid) && (cat.Product.Id == ) select cat).ToList();
                //PaymentModel = await _context.Payment.ToListAsync();
            }
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
