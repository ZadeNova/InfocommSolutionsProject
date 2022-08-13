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

        //public List<Categories> ProductsCategory { get; set; }

        public Dictionary<string,int> TopCategories { get; set; }

        public Dictionary<string, double> TopCategoriesBySales { get; set; }

        public double TotalSales { get; set; }

        public double AveragePricePerOrder { get; set; }

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
            TotalSales = Math.Round(_context.Orders.Sum(x => x.PriceOfOrder) , 2);
            AveragePricePerOrder = Math.Round((_context.Orders.Sum(x => x.PriceOfOrder) / _context.Orders.Count()), 2);
            
           // ProductsCategory = _context.Categories.Where(x => x.CategoryFor.ToLower() == "product" || x.CategoryFor.ToLower() == "products").ToList();
            
            var UserList = _userManager.Users.ToList();
            var testvar = from a in _userManager.Users select a.CreatedAt;
            UserDateList = UserList.Select(a => a.CreatedAt);


            TopCategories = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id select new { cat.Product , prod.Category}).GroupBy(x => x.Category).Select(y => new
            {
                Category = y.Key,
                Count = y.Count()
            }).ToDictionary(x => x.Category , x => x.Count);

            TopCategoriesBySales = (from cat in _context.Orders join prod in _context.Products on cat.Product.Id equals prod.Id select new { cat.Product, prod.Category , cat.PriceOfOrder }).GroupBy(x => x.Category).Select(y => new
            {
                Category = y.Key,
                Count = Math.Round(y.Sum( a => a.PriceOfOrder) , 2)
            }).ToDictionary(x => x.Category, x => x.Count);

            //foreach (var i in TopCategoriesBySales) System.Diagnostics.Debug.WriteLine($"{i.Key} , {i.Value}");

           
            //foreach (var i in getNumberOfCategory) System.Diagnostics.Debug.WriteLine($"{i.Category} , {i.Count}");


            //TopCategories = ProductsCategory.GroupBy(x => x.CategoryName).Select(g => new
            //{
            //    Category = g.Key,
            //    Count = g.Sum(x => x.CategoryFor == "Products")

            //})



            TopCustomers = OrdersList.GroupBy(x => x.Accounts).Select(g => new
            {
                Account = g.Key,
                Count = Math.Round(g.Sum(x => x.PriceOfOrder) , 2)
            }).OrderByDescending(a => a.Count).Take(5).ToDictionary(x => x.Account, x => x.Count);


            TopProducts_byAmount = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Count = Math.Round(g.Sum(x => x.PriceOfOrder) , 2)
            }).OrderByDescending(v => v.Count).Take(5).ToDictionary(m => m.Product.Name , m => m.Count);

            TopProducts_byQuantity = OrdersList.GroupBy(x => x.Product).Select(g => new
            {
                Product = g.Key,
                Quantity = g.Sum(x => x.quantity)

            }).OrderByDescending(x => x.Quantity).Take(5).ToDictionary(m => m.Product.Name, m => m.Quantity);



            //foreach(var i in TopCustomers) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value} wtfffff");
            //foreach (var i in TopProducts_byQuantity) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value}");
            //foreach (var b in Prods) System.Diagnostics.Debug.WriteLine($"{b.Key} {b.Value} Sussybaka");



        }
    }
}