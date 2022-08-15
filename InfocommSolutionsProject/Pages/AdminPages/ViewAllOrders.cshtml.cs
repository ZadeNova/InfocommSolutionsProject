using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;


namespace InfocommSolutionsProject.Pages.AdminPages
{
    public class ViewAllOrdersModel : PageModel
    {
        public const string SessionKeyName = "_userset";


        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        private readonly IConfiguration Configuration;

        public string CurrentFilter { get; set; }

        public string AccountsSort { get; set; }

        public string Price_Of_OrderSort { get; set; }

        public string DescriptionSort { get; set; }

        public string DateOfOrder_Sort { get; set; }

        public string ProductSort { get; set; }

        public string OrderStatus_Sort { get; set; }

        public string Address_Sort { get; set; }

        public string Quantity_Sort { get; set; }

        public string Notes_Sort { get; set; }
        public string shipping_Sort { get; set; }

        public string Current_NumRatingPerPage { get; set; }

        public string five { get; set; }

        public string ten { get; set; }

        public string fifty { get; set; }

        public string hundred { get; set; }

        public PaginatedList<OrdersModel> OrdersList { get; set; }

        public ViewAllOrdersModel(InfocommSolutionsProjectContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            
        }

        public async Task OnGetAsync(string searchString, int? pageIndex, string sortOrder, string NumPage)
        {
            AccountsSort = String.IsNullOrEmpty(sortOrder) ? "AccSort" : "";
            Price_Of_OrderSort = String.IsNullOrEmpty(sortOrder) ? "Price_Of_OrderSortDesc" : "Price_Of_OrderSortAscend";
            DateOfOrder_Sort = sortOrder == "dateorderAscend" ? "Date_DescCreated" : "dateorderAscend";
            ProductSort = String.IsNullOrEmpty(sortOrder) ? "ProductNameSortDesc" : "ProductNameSortAscend";
            OrderStatus_Sort = String.IsNullOrEmpty(sortOrder) ? "OrderSort" : "";
            Address_Sort = String.IsNullOrEmpty(sortOrder) ? "AddressSort" : "";
            Quantity_Sort = String.IsNullOrEmpty(sortOrder) ? "QuantitySort" : "";
            Notes_Sort = String.IsNullOrEmpty(sortOrder) ? "NotesSort" : "";
            shipping_Sort= String.IsNullOrEmpty(sortOrder) ? "shippingsort" : "";

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

            IQueryable<OrdersModel> OrdersQueryable = from P in _context.Orders.Include(x => x.Accounts).Include(y => y.Product) select P;

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
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.Accounts.Email);
                    break;
                case "shippingsort":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.FreeShipping);
                    break;
                case "Price_Of_OrderSortDesc":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.PriceOfOrder);
                    break;
                case "Price_Of_OrderSortAscend":
                    OrdersQueryable = OrdersQueryable.OrderBy(x => x.PriceOfOrder);
                    break;
                case "OrderSort":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.OrderStatus);
                    break;
                case "AddressSort":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.Address);
                    break;

                case "QuantitySort":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.quantity);
                    break;

                case "NotesSort":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.Notes);
                    break;


                case "ProductNameSortDesc":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.Product.Name);
                    break;

                case "ProductNameSortAscend":
                    OrdersQueryable = OrdersQueryable.OrderBy(x => x.Product.Name);
                    break;
                case "dateorderAscend":
                    OrdersQueryable = OrdersQueryable.OrderBy(x => x.DateOfOrder);
                    break;
                case "Date_DescCreated":
                    OrdersQueryable = OrdersQueryable.OrderByDescending(x => x.DateOfOrder);
                    break;
                default:
                    OrdersQueryable = OrdersQueryable.OrderBy(x => x.Accounts.Email);
                    break;

            }

            if (!String.IsNullOrEmpty(searchString))
            {
                OrdersQueryable = OrdersQueryable.Where(s => s.Notes.Contains(searchString) || s.Product.Name.Contains(searchString) || s.Accounts.Email.Contains(searchString) || s.Address.Contains(searchString));
            }
            
            if (_context.Orders != null)
            {

                if (Convert.ToInt32(Current_NumRatingPerPage) == 0)
                {
                    // the line below is to check for value inside SessionKeyName which my keyname is on this page line 17 //
                    if (string.IsNullOrEmpty((HttpContext.Session.GetString(SessionKeyName))))
                    {
                        //since the if above check for the value and if is now then the default value of the page will be 10 as below //
                        var pageSize = Configuration.GetValue("PageSize:", 10);
                        OrdersList = await PaginatedList<OrdersModel>.CreateAsync(OrdersQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                    }
                    else
                    {
                        //else if value of the sesson is not null then i will use that as the value for the page size as belwo //
                        var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(HttpContext.Session.GetString("_userset")));
                        OrdersList = await PaginatedList<OrdersModel>.CreateAsync(OrdersQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                    }

                }
                else
                {
                    //this is just to change the pages based on dropdown list //
                    var pageSize = Configuration.GetValue("PageSize:", Convert.ToInt32(Current_NumRatingPerPage));
                    OrdersList = await PaginatedList<OrdersModel>.CreateAsync(OrdersQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

                }


                //Product = await _context.Products.ToListAsync();
                //var pageSize = Configuration.GetValue("PageSize:", 4);
                //Product = await PaginatedList<ProductModel>.CreateAsync(ProductQueryable.AsNoTracking(), pageIndex ?? 1, pageSize);

            }


        }
    }
}
