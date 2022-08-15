using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace InfocommSolutionsProject.Pages.AdminPages
{
    public class ViewAllProductRatingsModel : PageModel
    {
        public const string SessionKeyName = "_userset";


        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        
        private readonly IConfiguration Configuration;

        public string Current_NumRatingPerPage { get; set; }

        public string five { get; set; }

        public string ten { get; set; }

        public string fifty { get; set; }

        public string hundred { get; set; }

        //public List<RatingsModel> AllRatings { get; set; }

        public PaginatedList<RatingsModel> AllRatings { get; set; }

        public List<ProductModel> ProductsList { get; set; }

        public List<Accounts> AllAccounts { get; set; }

        public string CurrentFilter { get; set; }

        public string AccountsSort { get; set; }

        public string RatingSort { get; set; }

        public string DescriptionSort { get; set; }

        public string Created_OnSort { get; set; }

        public string ProductSort { get; set; }

        public ViewAllProductRatingsModel(InfocommSolutionsProjectContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder, string NumPage)
        {
            AllAccounts = _context.Users.ToList();
            ProductsList = _context.Products.ToList();
            

            AccountsSort = String.IsNullOrEmpty(sortOrder) ? "AccSort" : "";
            RatingSort = String.IsNullOrEmpty(sortOrder) ? "RatingSortDesc" : "RatingSortAscend";
            DescriptionSort = String.IsNullOrEmpty(sortOrder) ? "DescriptionSort" : "";
            Created_OnSort = sortOrder == "datecreate" ? "Date_DescCreated" : "datecreate";
            ProductSort = String.IsNullOrEmpty(sortOrder) ? "ProductNameSortDesc" : "ProductNameSortAscend";

            // For Page sort
            Current_NumRatingPerPage = NumPage;
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

            IQueryable<RatingsModel> RatingQueryable = from P in _context.Ratings.Include(x => x.Accounts).Include(y => y.Product) select P;

            //var Rat = _context.Ratings.Include("Accounts");

            //foreach (var i in Rat) System.Diagnostics.Debug.WriteLine($"{i.Product.Name} {i.Product} ALDASDASEDASDADAW ");

            //foreach (var i in RatingQueryable) System.Diagnostics.Debug.WriteLine($"{i.Accounts} {i.Product} {i.CreatedOn}");

            switch (NumPage)
            {
                case "five":
                    Current_NumRatingPerPage = "5";
                    HttpContext.Session.SetString(SessionKeyName, "5");
                    break;
                case "ten":
                    Current_NumRatingPerPage = "10";
                    HttpContext.Session.SetString(SessionKeyName, "10");
                    break;
                case "hundred":
                    Current_NumRatingPerPage = "100";
                    HttpContext.Session.SetString(SessionKeyName, "100");
                    break;
                case "fifty":
                    Current_NumRatingPerPage = "50";
                    HttpContext.Session.SetString(SessionKeyName, "50");
                    break;


            }

            switch (sortOrder)
            {
                case "AccSort":
                    RatingQueryable = RatingQueryable.OrderByDescending(x => x.Accounts.Email);
                    break;
                case "RatingSortDesc":
                    RatingQueryable = RatingQueryable.OrderByDescending(x => x.rating);
                    break;
                case "RatingSortAscend":
                    RatingQueryable = RatingQueryable.OrderBy(x => x.rating);
                    break;
                case "DescriptionSort":
                    RatingQueryable = RatingQueryable.OrderByDescending(x => x.Description);
                    break;
                case "ProductNameSortDesc":
                    RatingQueryable = RatingQueryable.OrderByDescending(x => x.Product.Name);
                    break;

                case "ProductNameSortAscend":
                    RatingQueryable = RatingQueryable.OrderBy(x => x.Product.Name);
                    break;
                case "datecreate":
                    RatingQueryable = RatingQueryable.OrderBy(x => x.CreatedOn);
                    break;
                case "Date_DescCreated":
                    RatingQueryable = RatingQueryable.OrderByDescending(x => x.CreatedOn);
                    break;
                default:
                    RatingQueryable = RatingQueryable.OrderBy(x => x.Accounts.Email);
                    break;

            }


            if (!String.IsNullOrEmpty(searchString))
            {
                RatingQueryable = RatingQueryable.Where(s => s.Description.Contains(searchString) || s.Product.Name.Contains(searchString) || s.Accounts.Email.Contains(searchString));
            }


            if (_context.Ratings != null)
            {

                if (Convert.ToInt32(Current_NumRatingPerPage) == 0)
                {
                    // the line below is to check for value inside SessionKeyName which my keyname is on this page line 17 //
                    if (string.IsNullOrEmpty((HttpContext.Session.GetString(SessionKeyName))))
                    {
                        //since the if above check for the value and if is now then the default value of the page will be 10 as below //
                        var pageSize = Configuration.GetValue("PageSize:", 10);
                        AllRatings = await PaginatedList<RatingsModel>.CreateAsync(RatingQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                    }
                    else
                    {
                        //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                        var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(HttpContext.Session.GetString("_userset")));
                        AllRatings = await PaginatedList<RatingsModel>.CreateAsync(RatingQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                    }

                }
                else
                {
                    //this is just to change the pages based on dropdown list //
                    var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(Current_NumRatingPerPage));
                    AllRatings = await PaginatedList<RatingsModel>.CreateAsync(RatingQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                }

                
            }

         
            

            
        }
    }
}
