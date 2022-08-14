using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.CustomerPages
{
    public class ViewWishList : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public ViewWishList(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public IList<WishListModel> wishLists { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.wishLists != null)
            {
                wishLists = await _context.wishLists.ToListAsync();
                var lol = _context.Users.ToList();
                var idk = _context.Products.ToList();
            }
        }
    }
}
