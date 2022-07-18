using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace InfocommSolutionsProject.Pages.Product
{
    
    public class IndexModel : PageModel
    {
        // Session
        public const string SessionKeyName = "_userset";


        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // Used to filter columns by ascending or descending.
        public string ProdNameSort { get; set; }

        public string PriceSort { get; set; }

        public string CategorySort { get; set; }

        public string QuantitySort { get; set; }

        public string DiscountNumSort { get; set; }

        public string DateUpdatedSort { get; set; }

        // Used to show how many items are on the page

        public string Current_NumProdPerPage { get; set; }

        public string five { get; set; }

        public string ten { get; set; }

        public string fifty { get; set; }

        public string hundred { get; set; }

        




        public string CurrentFilter { get; set; }

        //public IList<ProductModel> Product { get;set; } = default!;

        public PaginatedList<ProductModel> Product { get; set; }



        public async Task OnGetAsync(string searchString , int? pageIndex , string sortOrder , string NumPage)
        {
            ProdNameSort = String.IsNullOrEmpty(sortOrder) ? "prodname" : "";
            PriceSort = String.IsNullOrEmpty(sortOrder) ? "PriceSortDesc" : "PriceSortAscend";
            CategorySort = String.IsNullOrEmpty(sortOrder) ? "CategorySort" : "";
            QuantitySort = String.IsNullOrEmpty(sortOrder) ? "QuantitySort" : "";
            DiscountNumSort = String.IsNullOrEmpty(sortOrder) ? "DiscNumSortDesc" : "DiscNumAscend";
            DateUpdatedSort = sortOrder == "Date" ? "date_desc" : "Date";

            // For Page sort
            Current_NumProdPerPage = NumPage;
            five = String.IsNullOrEmpty(NumPage) ? "five" : "";
            ten = String.IsNullOrEmpty(NumPage) ? "ten" : "";
            hundred = String.IsNullOrEmpty(NumPage) ? "hundred" : "";
            fifty = String.IsNullOrEmpty(NumPage) ? "fifty" : "";




            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<ProductModel> ProductQueryable = from P in _context.Products select P;

            switch (NumPage)
            {
                case "five":
                    Current_NumProdPerPage = "5";
                    HttpContext.Session.SetString(SessionKeyName, "5");
                    break;
                case "ten":
                    Current_NumProdPerPage = "10";
                    HttpContext.Session.SetString(SessionKeyName, "10");
                    break;
                case "hundred":
                    Current_NumProdPerPage = "100";
                    HttpContext.Session.SetString(SessionKeyName, "100");
                    break;
                case "fifty":
                    Current_NumProdPerPage = "50";
                    HttpContext.Session.SetString(SessionKeyName, "50");
                    break;
               

            }



            switch (sortOrder)
            {
                case "prodname":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.Name);
                    break;
                case "PriceSortDesc":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.Price);
                    break;
                case "PriceSortAscend":
                    ProductQueryable = ProductQueryable.OrderBy(x => x.Price);
                    break;
                case "CategorySort":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.Category);
                    break;
                case "QuantitySort":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.Quantity);
                    break;
                case "DiscNumSortDesc":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.Discount);
                    break;

                case "DiscNumAscend":
                    ProductQueryable = ProductQueryable.OrderBy(x => x.Discount);
                    break;

                case "Date":
                    ProductQueryable = ProductQueryable.OrderBy(x => x.UpdatedOn);
                    break;
                case "date_desc":
                    ProductQueryable = ProductQueryable.OrderByDescending(x => x.UpdatedOn);
                    break;
                default:
                    ProductQueryable = ProductQueryable.OrderBy(x => x.Name);
                    break;

            }

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductQueryable = ProductQueryable.Where(s => s.Name.Contains(searchString) || s.Category.Contains(searchString) || s.Description.Contains(searchString));
            }

            if (_context.Products != null)
            {

                if (Convert.ToInt32(Current_NumProdPerPage) == 0)
                {
                    // the line below is to check for value inside SessionKeyName which my keyname is on this page line 17 //
                    if (string.IsNullOrEmpty((HttpContext.Session.GetString(SessionKeyName))))
                    {
                        //since the if above check for the value and if is now then the default value of the page will be 10 as below //
                        var pageSize = Configuration.GetValue("PageSize:", 10);
                        Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                        
                    }
                    else
                    {
                        //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                        var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(HttpContext.Session.GetString("_userset")));
                        Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                        
                    }

                }
                else
                {
                    //this is just to change the pages based on dropdown list //
                    var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(Current_NumProdPerPage));
                    Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                    
                }


                //Product = await _context.Products.ToListAsync();
                //var pageSize = Configuration.GetValue("PageSize:", 4);
                //Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

            }

            

        }
    }
}
