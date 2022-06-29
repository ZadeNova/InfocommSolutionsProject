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

namespace InfocommSolutionsProject.Pages
{
    public class AdminPageModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public AdminPageModel(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public int NumberOfProducts { get; set; }

        public int NumberOfSuppliers { get; set; }

        public int NumberOfSupplierOrders { get; set; }

        public int NumberOfProductOrders { get; set; }

        public void OnGet()
        {
            NumberOfProducts = _context.Products.Count();
            NumberOfSuppliers = _context.Suppliers.Count();
            NumberOfProductOrders = _context.Orders.Count();
            NumberOfSupplierOrders = _context.SupplierOrders.Count();

            //System.Diagnostics.Debug.WriteLine($"{NumberOfProducts} HZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");

        }
    }
}