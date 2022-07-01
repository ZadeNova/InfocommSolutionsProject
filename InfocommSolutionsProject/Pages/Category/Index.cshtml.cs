using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Category
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
        public string CurrentFilter { get; set; }
        public PaginatedList<Categories> categore { get; set; }
        //public IList<Categories> Categories { get;set; } = default!;

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

            IQueryable<Categories> CateQueryable = from P in _context.Categories select P;

            if (!String.IsNullOrEmpty(searchString))
            {
                CateQueryable = CateQueryable.Where(s => s.CategoryName.Contains(searchString) || s.CategoryFor.Contains(searchString) || s.Description.Contains(searchString));
            }
            if (_context.Categories != null)
            {
                var pageSize = Configuration.GetValue("PageSize:", 10);
                categore = await PaginatedList<Categories>.CreateAsync(CateQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                //Categories = await _context.Categories.ToListAsync();
            }
        }
    }
}
