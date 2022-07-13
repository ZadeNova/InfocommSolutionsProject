using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Supplier
{
    public class IndexModel : PageModel
    {
        public const string SessionKeyName = "_usersetsupplier";
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

       // public IList<SupplierModel> SupplierModel { get;set; } = default!;
        public string CurrentFilter { get; set; }
        public PaginatedList<SupplierModel> suppler { get; set; }
        public string currentPagesort { get; set; }
        public string five { get; set; }
        public string ten { get; set; }
        public string fivete { get; set; }
        public string hun { get; set; }

        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder1, int currentFilter1)
        {
            currentPagesort = sortOrder1;
            five = String.IsNullOrEmpty(sortOrder1) ? "five" : "";
            ten = String.IsNullOrEmpty(sortOrder1) ? "ten" : "";
            fivete = String.IsNullOrEmpty(sortOrder1) ? "fivete" : "";
            hun = String.IsNullOrEmpty(sortOrder1) ? "hun" : "";


            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            switch (sortOrder1)
            {
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

            IQueryable<SupplierModel> SupplierQueryable = from P in _context.Suppliers select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                SupplierQueryable = SupplierQueryable.Where(s => s.SupplierName.Contains(searchString) || s.SupplierCategory.Contains(searchString));
            }
            if (_context.Suppliers != null)
            {
                if (Convert.ToInt32(currentPagesort) == 0)
                {
                    // the line below is to check for value inside SessionKeyName which my keyname is on this page line 17 //
                    if (string.IsNullOrEmpty((HttpContext.Session.GetString(SessionKeyName))))
                    {
                        //since the if above check for the value and if is now then the default value of the page will be 10 as below //
                        var pageSize = Configuration.GetValue("PageSize:", 10);
                        suppler = await PaginatedList<SupplierModel>.CreateAsync(SupplierQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                    }
                    else
                    {
                        //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                        var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(HttpContext.Session.GetString("_usersetsupplier")));
                        suppler = await PaginatedList<SupplierModel>.CreateAsync(SupplierQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                    }

                }
                else
                {
                    //this is just to change the pages based on dropdown list //
                    var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(currentPagesort));
                    suppler = await PaginatedList<SupplierModel>.CreateAsync(SupplierQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                }


                //SupplierModel = await _context.Suppliers.ToListAsync();
            }
        }
    }
}
