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

        public async Task OnGetAsync(string searchString, int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<SupplierModel> SupplierQueryable = from P in _context.Suppliers select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                SupplierQueryable = SupplierQueryable.Where(s => s.SupplierName.Contains(searchString) || s.SupplierCategory.Contains(searchString));
            }
            if (_context.Suppliers != null)
            {
                var pageSize = Configuration.GetValue("PageSize", 10);
                suppler = await PaginatedList<SupplierModel>.CreateAsync(SupplierQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                //SupplierModel = await _context.Suppliers.ToListAsync();
            }
        }
    }
}
