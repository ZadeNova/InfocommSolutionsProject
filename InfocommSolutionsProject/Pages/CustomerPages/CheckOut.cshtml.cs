using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using InfocommSolutionsProject.Data;


namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class CheckOutModel : PageModel
    {
        public List<ShoppingCartItem> TheShoppingCart { get; set; }

        public double TotalCost { get; set; }

        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public CheckOutModel(InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Check if shopping cart have items.
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            if (TheShoppingCart == null)
            {

            }
            else
            {
                // If cart has items calculate the total 
                foreach (var item in TheShoppingCart)
                {
                    if (item.Product.DiscountStatus == true)
                    {
                        TotalCost += (item.Product.Price - (item.Product.Price * item.Product.Discount / 100)) * item.Quantity;
                    }
                    else
                    {
                        TotalCost += item.Product.Price * item.Quantity;
                    }

                }
            }




        }
    }
}
