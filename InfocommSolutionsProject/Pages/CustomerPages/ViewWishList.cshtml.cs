﻿using System;
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
    public class ViewWishList : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;
        private readonly UserManager<Accounts> _userManager;
        public ViewWishList(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context, UserManager<Accounts> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<WishListModel> wishLists { get;set; } = default!;
        public List<ShoppingCartItem> TheShoppingCart { get; set; }
        public ProductModel Product { get; set; } = default!;
        [BindProperty]
        public WishListModel wishList123 { get; set; } = default!;
        public string userid { get; set; }
        private Task<Accounts> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public async Task<string> GetCurrentUserId()
        {
            Accounts usr = await GetCurrentUserAsync();
            userid = usr.Id;

            return usr.Id;
        }
        public async Task OnGetAsync()
        {
            if (_context.wishLists != null)
            {
                await GetCurrentUserId();
                wishLists = await _context.wishLists.ToListAsync();
                var lol = _context.Users.ToList();
                var idk = _context.Products.ToList();
            }
        }



        public async Task<IActionResult> OnPostAddToShoppingCart(string id, int ItemQuantity)
        {
            Guid id1 = Guid.Parse(id);
            var product = _context.Products.FirstOrDefault(m => m.Id == id1);
            Product = product;
            TheShoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "ShoppingCart");
            var wishlist = _context.wishLists.Where(i => i.Product.Id == id1).Where(x => x.Accounts.Id == userid);
            if (wishLists != null)
            {
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
                //wishList123.Product.Id = id1;
                //_context.wishLists.Remove(wishList123).State = EntityState.Modified;
                //await _context.SaveChangesAsync();

            }

            //return RedirectToPage("ProductDetails" + id);
            return Redirect("~/CustomerPages/ViewWishList");

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
