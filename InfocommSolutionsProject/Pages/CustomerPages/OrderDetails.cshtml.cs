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

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class OrderDetails : PageModel
    {
        public const string SessionKeyName = "_userset";
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        private readonly IConfiguration Configuration;
        public OrderDetails(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            Configuration = configuration;
            
        }
        // Used to filter columns by ascending or descending.
        public string EmailSort { get; set; }
        public string DateOfOrderSort { get; set; }
        public string OrderStatusSort { get; set; }
        public string NotesSort { get; set; }

        public string shipping { get; set; }



        // Used to show how many items are on the page

        public string Current_NumProdPerPage { get; set; }
        public string five { get; set; }
        public string ten { get; set; }
        public string fifty { get; set; }
        public string hundred { get; set; }
        public string CurrentFilter { get; set; }


        public PaginatedList<OrdersModel> OrderDetail { get;set; } = default!;
        public IList<Accounts> Accounts { get; set; } = default!;
        public string userid { get; set; }
        //public IList<PaymentModel> UserSelectList { get; set; }
        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder, string NumPage)
        {
            if (_context.Payment != null)
            {
                //get current login user id 
                await GetCurrentUserId();
             
                // get the userid from the above function and get the user payment information out only 
                //OrderDetail = await _context.Orders.Where(i => i.Accounts.Id == userid).ToListAsync();
                //Products = _context.Products.ToList();

                //OrderDetail = (from cat in _context.Orders where (cat.Accounts.Id==userid) && (cat.Product.Id == ) select cat).ToList();
                //PaymentModel = await _context.Payment.ToListAsync();

                EmailSort = String.IsNullOrEmpty(sortOrder) ? "Email" : "";
                OrderStatusSort = String.IsNullOrEmpty(sortOrder) ? "OrderStatusSort" : "";
                NotesSort = String.IsNullOrEmpty(sortOrder) ? "NotesSortSort" : "";
                DateOfOrderSort = sortOrder == "Date" ? "date_desc" : "Date";
                shipping= String.IsNullOrEmpty(sortOrder) ? "ship" : "";

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
                Accounts = await _context.Users.Where(i => i.Id == userid).ToListAsync();
                IQueryable<OrdersModel> ProductQueryable = from cat in _context.Orders join dog in _context.Users on cat.Accounts.Id equals dog.Id where (cat.Accounts.Id == userid)  select cat;

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
                    case "Email":
                        ProductQueryable = ProductQueryable.OrderByDescending(x => x.Accounts.Email);
                        break;
                    case "ship":
                        ProductQueryable = ProductQueryable.OrderByDescending(x => x.FreeShipping);
                        break;
                    case "OrderStatusSort":
                        ProductQueryable = ProductQueryable.OrderByDescending(x => x.OrderStatus);
                        break;
                    case "NotesSortSort":
                        ProductQueryable = ProductQueryable.OrderByDescending(x => x.Notes);
                        break;
                    case "Date":
                        ProductQueryable = ProductQueryable.OrderBy(x => x.DateOfOrder);
                        break;
                    case "date_desc":
                        ProductQueryable = ProductQueryable.OrderByDescending(x => x.DateOfOrder);
                        break;
                    default:
                        ProductQueryable = ProductQueryable.OrderBy(x => x.Accounts.Email);
                        break;

                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    ProductQueryable = ProductQueryable.Where(s => s.Accounts.Email.Contains(searchString) || s.OrderStatus.Contains(searchString) || s.Notes.Contains(searchString));
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
                            OrderDetail = await PaginatedList<OrdersModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                        }
                        else
                        {
                            //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                            var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(HttpContext.Session.GetString("_userset")));
                            OrderDetail = await PaginatedList<OrdersModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);
                        }

                    }
                    else
                    {
                        //this is just to change the pages based on dropdown list //
                        var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(Current_NumProdPerPage));
                        OrderDetail = await PaginatedList<OrdersModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                    }

                }
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
