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

namespace InfocommSolutionsProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<PaymentModel> PaymentModel { get;set; } = default!;
        public string userid { get; set; }
        //public IList<PaymentModel> UserSelectList { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Payment != null)
            {
                //get current login user id 
                await GetCurrentUserId();
                // get the userid from the above function and get the user payment information out only 
                PaymentModel = await _context.Payment.Where(i => i.Accounts.Id==userid).ToListAsync();
               
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
