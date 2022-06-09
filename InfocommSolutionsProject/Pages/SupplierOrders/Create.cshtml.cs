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

namespace InfocommSolutionsProject.Pages.SupplierOrders
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<Accounts> _userManager;

        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public CreateModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult OnGet()
        {

            //var UserData = (from Users in _context.Users select Users.UserName).ToList();

            //ViewData["AccountsList"] = UserData;
            //foreach (var i in Models.Accounts) System.Diagnostics.Debug.WriteLine(i);
            PopulateUserList(_context);
            PopulateSupplierList(_context);
            return Page();
        }
        public void PopulateSupplierList(InfocommSolutionsProjectContext _context, object userobj = null)
        {
            var supplerdata = from SupplierModel in _context.Suppliers select SupplierModel;
            SupplierList = new SelectList(supplerdata, "SupplierId", "SupplierName", userobj);
        }
        public void PopulateUserList(InfocommSolutionsProjectContext _context , object userobj = null)
        {
            var UserData = from user in _context.Users select user;
            UserSelectList = new SelectList(UserData, "Id","UserName", userobj);
        }

        [BindProperty]
        public SupplierOrdersModel SupplierOrdersModel { get; set; } = default!;

        public SelectList UserSelectList { get; set; }
        public SelectList SupplierList { get; set; }
        public string aid { get; set; }
        public string sid { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            aid = Request.Form["accid"].ToString();
            sid = Request.Form["suppieid"].ToString();
            //SupplierOrdersModel.AccountID = (from users in _context.Users where users.Id.Equals("0e1de4b2-e453-4f8b-b81d-3e8f875f42bb") select users) ;
            SupplierOrdersModel.AccountID = _context.Users.First(i => i.Id == aid);
            SupplierOrdersModel.SupplierID = _context.Suppliers.First(i => i.SupplierId.ToString() == sid);
            //SupplierOrdersModel.SupplierID = "F5163474-AE47-4F30-FD9A-08DA48A8718D";
            //System.Diagnostics.Debug.WriteLine(SupplierOrdersModel.OrderStatus);
            System.Diagnostics.Debug.WriteLine(SupplierOrdersModel.AccountID);
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

          //if (!ModelState.IsValid || _context.SupplierOrders == null || SupplierOrdersModel == null)
          //  {

          //      System.Diagnostics.Debug.WriteLine(ModelState.IsValid);
          //      System.Diagnostics.Debug.WriteLine(_context.SupplierOrders == null);
          //      System.Diagnostics.Debug.WriteLine(SupplierOrdersModel == null);
                
          //      System.Diagnostics.Debug.WriteLine("TESTING");
          //      return Page();
                
          //  }
            

            _context.SupplierOrders.Add(SupplierOrdersModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
