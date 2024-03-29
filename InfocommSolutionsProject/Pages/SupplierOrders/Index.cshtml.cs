﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.SupplierOrders
{
    public class IndexModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public IndexModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        public IList<SupplierOrdersModel> SupplierOrdersModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SupplierOrders != null)
            {
                SupplierOrdersModel = await _context.SupplierOrders.ToListAsync();
            }
        }
    }
}
