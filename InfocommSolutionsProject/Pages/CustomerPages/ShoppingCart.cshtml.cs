using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Models;
using InfocommSolutionsProject.Data;
using Microsoft.EntityFrameworkCore;



namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class ShoppingCartModel : PageModel
    {
        public List<ShoppingCartItem> TheShoppingCart { get; set; }

        public double TotalCost { get; set; }

        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
    
        public ShoppingCartModel(InfocommSolutionsProjectContext context)
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
                        TotalCost += (item.Product.Price - (item.Product.Price * item.Product.Discount / 100))  * item.Quantity;
                    }
                    else
                    {
                        TotalCost += item.Product.Price * item.Quantity;
                    }

                }
            }
            



            



        }

        public IActionResult OnGetBuyNow(string id)
        {
         
            System.Diagnostics.Debug.WriteLine("The Shopping Carttrtt");
            System.Diagnostics.Debug.WriteLine(id);
            var ProductModel = new ProductModel();

            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");

            if (TheShoppingCart == null)
            {
                TheShoppingCart = new List<ShoppingCartItem>();
                TheShoppingCart.Add(
                    new ShoppingCartItem
                    {
                        Product = _context.Products.Find(Guid.Parse(id)),
                        Quantity = 1
                    });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);

            }
            else
            {
                int index = Exists(TheShoppingCart, id);
                if (index == -1)
                {
                    TheShoppingCart.Add(new ShoppingCartItem
                    {
                       Product = _context.Products.Find(Guid.Parse(id)),
                       Quantity = 1

                    });
                }
                else
                {
                    TheShoppingCart[index].Quantity++;

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);



            }
            return RedirectToPage("ShoppingCart");

        }

        public IActionResult OnGetDelete(string id)
        {
            System.Diagnostics.Debug.WriteLine("The delete button lol");
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            int index = Exists(TheShoppingCart, id);
            TheShoppingCart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);
            return RedirectToPage("ShoppingCart");
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {

            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            if (TheShoppingCart is null)
            {
                return RedirectToPage("Shop");
            }
            else {
                for (var i = 0; i < TheShoppingCart.Count; i++)
                {
                    TheShoppingCart[i].Quantity = quantities[i];
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);
                return RedirectToPage("ShoppingCart");
            }
            
        }




        private int Exists(List<ShoppingCartItem> cart, string id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.ToString() == id)
                {
                    return i;
                }
            }
            return -1;
        }


    }
}
