using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;

namespace InfocommSolutionsProject.Areas.Identity.Pages.Account.Manage
{
    public class CreateModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public CreateModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        //public IActionResult OnGet()
        //{
        //    PopulateUserList(_context);

        //    return Page();
        //}

        public async Task OnGet()
        {
            await GetCurrentUserId();
            PopulateUserList(_context);
        }
        [BindProperty]
        public PaymentModel PaymentModel { get; set; } = default!;

        public SelectList UserSelectList { get; set; }
        public string aid { get; set; }
        public string username { get; set; }

        public void PopulateUserList(InfocommSolutionsProjectContext _context, object userobj = null)
        {
            var UserData = from user in _context.Users select user;
            UserSelectList = new SelectList(UserData, "UserName", "Id", userobj);
        }

        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            aid = usr.Id;

            return usr?.Id;
        }

        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // aid = Request.Form["accid"];
            // var idk = _context.Users.First(i => i.Id == aid);
            await GetCurrentUserId();
            PaymentModel.Accounts = _context.Users.First(i => i.Id == aid);
            System.Diagnostics.Debug.WriteLine(PaymentModel.Accounts);

            // i remove this cuz the raw value of this paymentmodel.accounts raw value = null due to the virtual clas 

            //if (!ModelState.IsValid || _context.Payment == null || PaymentModel == null)
            //{
            //    return Page();
            //}
          
        
            _context.Payment.Add(PaymentModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PaymentIndex");
        }
    }
}
