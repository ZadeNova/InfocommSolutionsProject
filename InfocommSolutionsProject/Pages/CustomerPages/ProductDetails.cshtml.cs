using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Models;
using InfocommSolutionsProject.Data;

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;



        public ProductDetailsModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public ProductModel Product { get; set; } = default!;

        public List<ShoppingCartItem> TheShoppingCart { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
            
        }

        public IActionResult OnPostAddToShoppingCart(string id , int ItemQuantity)
        {
            System.Diagnostics.Debug.WriteLine($"This is the Item Quantity {ItemQuantity} SUSSSS");

            System.Diagnostics.Debug.WriteLine("Activating from ProductDetails Shopping Cart Side");
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            if (TheShoppingCart == null)
            {
                TheShoppingCart = new List<ShoppingCartItem>();
                TheShoppingCart.Add(
                    new ShoppingCartItem
                    {
                        Product = _context.Products.Find(Guid.Parse(id)),
                        Quantity = ItemQuantity
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
                        Quantity = ItemQuantity

                    });
                }
                else
                {
                    TheShoppingCart[index].Quantity += ItemQuantity;

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ShoppingCart", TheShoppingCart);



            }




            //return RedirectToPage("ProductDetails" + id);
            return Redirect("~/CustomerPages/ProductDetails?id=" + id);

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

