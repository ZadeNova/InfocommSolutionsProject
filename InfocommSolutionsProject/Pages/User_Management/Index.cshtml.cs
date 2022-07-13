using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfocommSolutionsProject.Pages.User_Management
{
    public class IndexModel : PageModel
    {
        //createnew session key name as _userset//
        public const string SessionKeyName = "_userset";
     

        private readonly UserManager<Accounts> _userManager;
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IConfiguration configuration, UserManager<Accounts> userManager)
        {
            _context = context;
            Configuration = configuration;
            _userManager = userManager;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<Accounts> account { get; set; }
     
        //sortting 
        public string CurrentSort { get; set; }
        public string TwofaSort { get; set; }
        public string DateSort { get; set; }
        public string failcount { get; set; }
        public string accstatus { get; set; }
        public string lockouten { get; set; }
        public string  currentPagesort { get; set; }
        //public string Userpageint { get; set; }
        public string five { get; set; }
        public string ten { get; set; }
        public string fivete { get; set; }
        public string hun { get; set; }

        // public List<SelectListItem> UserSetItemPerPage { get; set; }
        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder, string currentFilter ,string sortOrder1, int currentFilter1)
        {
            
            
            CurrentSort = sortOrder;
            currentPagesort= sortOrder1;
            five=String.IsNullOrEmpty(sortOrder1) ? "five" : "";
            ten = String.IsNullOrEmpty(sortOrder1) ? "ten" : "";
            fivete = String.IsNullOrEmpty(sortOrder1) ? "fivete" : "";
            hun = String.IsNullOrEmpty(sortOrder1) ? "hun" : "";

            // System.Diagnostics.Debug.WriteLine(currentPagesort,"dadad");
            TwofaSort = String.IsNullOrEmpty(sortOrder) ? "two_fa" : "";
            failcount = String.IsNullOrEmpty(sortOrder) ? "fail_count" : "";
            lockouten = String.IsNullOrEmpty(sortOrder) ? "lockout_en" : "";
            accstatus = String.IsNullOrEmpty(sortOrder) ? "acc_status" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Accounts> accountQueryable = from P in _userManager.Users select P;
            //sortorder 1 is for me to let user decide how many record they wan to view on that page//
            switch (sortOrder1) {
                case "five":
                    currentPagesort = "5";
                    HttpContext.Session.SetString(SessionKeyName, "5");
                    break;
                case "ten":
                    currentPagesort = "10";
                    HttpContext.Session.SetString(SessionKeyName, "10");
                    break;
                case "fivete":
                    currentPagesort = "50";
                    HttpContext.Session.SetString(SessionKeyName, "50");
                    break;
                case "hun":
                    currentPagesort = "100";
                    HttpContext.Session.SetString(SessionKeyName, "100");
                    break;
              
            }
            switch (sortOrder)
            {
                case "two_fa":
                    accountQueryable = accountQueryable.OrderByDescending(p => p.TwoFactorEnabled);
                    break;
                case "fail_count":
                    accountQueryable = accountQueryable.OrderByDescending(p => p.AccessFailedCount);
                    break;
                case "acc_status":
                    accountQueryable = accountQueryable.OrderByDescending(p => p.AccountStatus);
                    break;
                case "lockout_en":
                    accountQueryable = accountQueryable.OrderByDescending(p => p.LockoutEnabled);
                    break;
                case "Date":
                    accountQueryable = accountQueryable.OrderBy(p => p.LockoutEnd);
                    break;
                case "date_desc":
                    accountQueryable = accountQueryable.OrderByDescending(p => p.LockoutEnd);
                    break;
                default:
                    accountQueryable = accountQueryable.OrderBy(p => p.Email);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                accountQueryable = accountQueryable.Where(s => s.UserName.Contains(searchString));
            }
            if (_userManager.Users != null)
            {
                System.Diagnostics.Debug.WriteLine(currentPagesort, "dadad");
                if (Convert.ToInt32(currentPagesort) == 0)
                {
                    // the line below is to check for value inside SessionKeyName which my keyname is on this page line 17 //
                    if (string.IsNullOrEmpty((HttpContext.Session.GetString(SessionKeyName))))
                    { 
                        //since the if above check for the value and if is now then the default value of the page will be 10 as below //
                        var pageSize = Configuration.GetValue("PageSize:", 10);
                        account = await PaginatedList<Accounts>.CreateAsync(accountQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                    }
                    else {
                        //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                        var pageSize = Configuration.GetValue("PageSize:",Convert.ToInt32(HttpContext.Session.GetString("_userset")));
                        account = await PaginatedList<Accounts>.CreateAsync(accountQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                    }
                    
                }
                else {
                    //this is just to change the pages based on dropdown list //
                    var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(currentPagesort));
                    account = await PaginatedList<Accounts>.CreateAsync(accountQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                }




                //SupplierModel = await _context.Suppliers.ToListAsync();

            }
        }
        
    }
}