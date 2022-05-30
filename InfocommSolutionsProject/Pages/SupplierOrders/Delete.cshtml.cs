using System;
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
    public class DeleteModel : PageModel
    {
        private readonly InfocommSolutionsProject.Data.InfocommSolutionsProjectContext _context;

        public DeleteModel(InfocommSolutionsProject.Data.InfocommSolutionsProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SupplierOrdersModel SupplierOrdersModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.SupplierOrders == null)
            {
                return NotFound();
            }

            var supplierordersmodel = await _context.SupplierOrders.FirstOrDefaultAsync(m => m.SupplierOrderId == id);

            if (supplierordersmodel == null)
            {
                return NotFound();
            }
            else 
            {
                SupplierOrdersModel = supplierordersmodel;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.SupplierOrders == null)
            {
                return NotFound();
            }
            var supplierordersmodel = await _context.SupplierOrders.FindAsync(id);

            if (supplierordersmodel != null)
            {
                SupplierOrdersModel = supplierordersmodel;
                _context.SupplierOrders.Remove(SupplierOrdersModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
