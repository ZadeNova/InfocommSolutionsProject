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

        public AdminPageModel(InfocommSolutionsProjectContext context,UserManager<Accounts> userManager)
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

        public Dictionary<Accounts,double> TopCustomers { get; set; }
        

        public void OnGet()
        {
            NumberOfProducts = _context.Products.Count();
            NumberOfSuppliers = _context.Suppliers.Count();
            NumberOfProductOrders = _context.Orders.Count();
            NumberOfSupplierOrders = _context.SupplierOrders.Count();
            NumberOfUsersInWebApplication = _userManager.Users.Count();

            var UserList = _userManager.Users.ToList();
            var testvar = from a in _userManager.Users select a.CreatedAt;
            UserDateList = UserList.Select(a => a.CreatedAt);


            //var Test = from Acc in _context.Orders select Acc.Accounts;
            var lol = _context.Orders.ToList();
            
            System.Diagnostics.Debug.WriteLine(lol.Count());
            //var final = from l in lol group l by l.Product into g select new { Product = g.Key, Count = (from l in g select l.Product).Distinct().Count() };

            //var final = lol.GroupBy(x => x.Product).Select(p => new
            //{
            //    Product = p.Key,
            //    TotalQuantity = p.Sum(x => x.quantity)
            //});

            //var final = from item in _context.Orders.ToList() group item by new { item.Product ,item.quantity } into itemsummary select new { Product = itemsummary.Key.Product, Count = itemsummary.Key.quantity };

            //var final = 

            // foreach (var i in final) System.Diagnostics.Debug.WriteLine($"{i.Account} {i.Count} wtfffff");
            //System.Diagnostics.Debug.WriteLine(final.Count());
            TopCustomers = lol.GroupBy(x => x.Accounts).Select(g => new
            {
                Account = g.Key,
                Count = g.Sum(x => x.quantity * x.PriceOfOrder)
            }).OrderByDescending(a => a.Count).ToDictionary(x => x.Account , x => x.Count);

            foreach(var i in TopCustomers) System.Diagnostics.Debug.WriteLine($"{i.Key} {i.Value} wtfffff");

            //foreach (var l in lol) System.Diagnostics.Debug.WriteLine($"{l.Product.Name} {l.quantity}");
            //foreach (var i in UserDates) System.Diagnostics.Debug.WriteLine($"{i} fuck u");

            //System.Diagnostics.Debug.WriteLine($"{NumberOfProducts} HZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");

        }
    }
}