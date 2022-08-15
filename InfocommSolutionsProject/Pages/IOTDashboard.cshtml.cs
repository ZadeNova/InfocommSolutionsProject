using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Net;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;

namespace InfocommSolutionsProject.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        private readonly UserManager<Accounts> _userManager;

        public int NumberOfProducts { get; set; }

        public int NumberOfSuppliers { get; set; }

        public int NumberOfSupplierOrders { get; set; }

        public int NumberOfProductOrders { get; set; }

        public int NumberOfUsersInWebApplication { get; set; }

        public double TotalSales { get; set; }

        public double AveragePricePerOrder { get; set; }

        public int Accounts_That_Have_Wishlist { get; set; }


        public DashboardModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://infocommsolutionsprojectsgae.scm.azurewebsites.net/api/triggeredwebjobs/IOTSim/run");
            request.Method = "POST";
            var byteArray = Encoding.ASCII.GetBytes("$InfocommSolutionsProjectSGAE:njTfurAvwCtcJtDamfdbTL4njqczqyicBdEbN7G3lZurQDxfbjZ5kSQvx2WM"); //we could find user name and password in Azure web app publish profile 
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
            request.ContentLength = 0;

            TotalSales = Math.Round(_context.Orders.Sum(x => x.PriceOfOrder), 2);
            AveragePricePerOrder = Math.Round((_context.Orders.Sum(x => x.PriceOfOrder) / _context.Orders.Count()), 2);
            Accounts_That_Have_Wishlist = (_context.wishLists.Where(x => x.Status == "Waiting").Select(x => x.Accounts).Distinct()).Count();

            NumberOfProducts = _context.Products.Count();
            NumberOfSuppliers = _context.Suppliers.Count();
            NumberOfProductOrders = _context.Orders.Count();
            NumberOfSupplierOrders = _context.SupplierOrders.Count();
            NumberOfUsersInWebApplication = _userManager.Users.Count();


            try
            {
                var response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {

            }
        }
    }
}
