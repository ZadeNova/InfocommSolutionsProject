using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace InfocommSolutionsProject.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        private readonly UserManager<Accounts> _userManager;

        public AdminPageModel(InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int NumberOfProducts { get; set; }

        public int NumberOfSuppliers { get; set; }

        public int NumberOfSupplierOrders { get; set; }

        public int NumberOfProductOrders { get; set; }

        public int NumberOfUsersInWebApplication { get; set; }

        public IEnumerable<DateTime>? UserDateList { get; set; }

        public Dictionary<Accounts, double> TopCustomers { get; set; }

        public Dictionary<string, double> TopProducts_byAmount { get; set; }

        public Dictionary<string, int> TopProducts_byQuantity { get; set; }

        public List<OrdersModel> OrdersList { get; set; }

        public List<ProductModel> ProductList { get; set; }

        public List<OrdersModel> LatestOrders { get; set; }

        public List<Categories> ProductsCategory { get; set; }

        public Dictionary<string,int> TopCategories { get; set; }

        public void OnGet()
        {
            // Assigning the variables from database
            NumberOfProducts = _context.Products.Count();
            NumberOfSuppliers = _context.Suppliers.Count();
            NumberOfProductOrders = _context.Orders.Count();
            NumberOfSupplierOrders = _context.SupplierOrders.Count();
            NumberOfUsersInWebApplication = _userManager.Users.Count();
            OrdersList = _context.Orders.ToList();
            ProductList = _context.Products.ToList();
            LatestOrders = _context.Orders.OrderByDescending(x => x.DateOfOrder).Take(8).ToList();
            ProductsCategory = _context.Categories.Where(x => x.CategoryFor.ToLower() == "product" || x.CategoryFor.ToLower() == "products").ToList();
            
            var UserList = _userManager.Users.ToList();
            var testvar = from a in _userManager.Users select a.CreatedAt;
            UserDateList = UserList.Select(a => a.CreatedAt);





            //TopCategories = ProductsCategory.GroupBy(x => x.CategoryName).Select(g => new
            //{
            //    Category = g.Key,
            //    Count = g.Sum(x => x.CategoryFor == "Products")

            //})


         
            TopCustomers = OrdersList.GroupBy(x => x.Accounts).Select(g => new
            {
                Account = g.Key,
                Count = g.Sum(x => x.quantity * x.PriceOfOrder)
            }).OrderByDescending(a => a.Count).Take(5).ToDictionary(x => x.Account , x => x.Count);


            TopProducts_byAmount = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Count = g.Sum(x => x.quantity * x.PriceOfOrder)
            }).OrderByDescending(v => v.Count).Take(5).ToDictionary(m => m.Product.Name , m => m.Count);

            TopProducts_byQuantity = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Quantity = g.Sum(x => x.quantity)

            }).OrderByDescending(x => x.Quantity).Take(5).ToDictionary(m => m.Product.Name, m => m.Quantity);



            //foreach(var i in TopCustomers) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value} wtfffff");
            foreach (var i in TopProducts_byQuantity) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value}");
            //foreach (var b in Prods) System.Diagnostics.Debug.WriteLine($"{b.Key} {b.Value} Sussybaka");



        }
    }
}