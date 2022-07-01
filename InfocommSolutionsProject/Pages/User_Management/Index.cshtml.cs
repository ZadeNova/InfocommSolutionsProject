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


namespace InfocommSolutionsProject.Pages.User_Management
{
    public class IndexModel : PageModel
    {
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

        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder, string currentFilter)
        {
            CurrentSort = sortOrder;
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
                var pageSize = Configuration.GetValue("PageSize:", 10);
                account = await PaginatedList<Accounts>.CreateAsync(accountQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                //SupplierModel = await _context.Suppliers.ToListAsync();
                
            }
        }
    }
}