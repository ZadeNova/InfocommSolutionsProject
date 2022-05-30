﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InfocommSolutionsProject.Data;
using InfocommSolutionsProject.Models;

namespace InfocommSolutionsProject.Pages.Supplier
{
    public class DeleteModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public DeleteModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SupplierModel SupplierModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var suppliermodel = await _context.Suppliers.FirstOrDefaultAsync(m => m.SupplierId == id);

            if (suppliermodel == null)
            {
                return NotFound();
            }
            else 
            {
                SupplierModel = suppliermodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }
            var suppliermodel = await _context.Suppliers.FindAsync(id);

            if (suppliermodel != null)
            {
                SupplierModel = suppliermodel;
                _context.Suppliers.Remove(SupplierModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
