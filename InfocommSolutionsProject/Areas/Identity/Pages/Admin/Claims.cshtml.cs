using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfocommSolutionsProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InfocommSolutionsProject.Areas.Identity.Pages.Admin
{
    public class ClaimsModel : PageModel
    {
        public UserManager<Accounts> _UserManager { get; set; }

        public ClaimsModel(UserManager<Accounts> managerr)
        {
            _UserManager = managerr;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("/");
            }
            Accounts user = await _UserManager.FindByIdAsync(Id);
            Claims = await _UserManager.GetClaimsAsync(user);
            return Page();

        }

        public async Task<IActionResult> OnPostAddClaimAsync([Required] string type , [Required] string value)
        {
            Accounts user = await _UserManager.FindByIdAsync(Id);

            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);
                var result = await _UserManager.AddClaimAsync(user, claim);
                if (!result.Succeeded)
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            Claims = await _UserManager.GetClaimsAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditClaimAsync([Required] string type,
                                                             [Required] string value,
                                                             [Required] string oldValue)
        {
            Accounts user = await _UserManager.FindByIdAsync(Id);
            if (ModelState.IsValid)
            {
                var claimNew = new Claim(type, value);
                var claimOld = new Claim(type, oldValue);
                var result = await _UserManager.ReplaceClaimAsync(user, claimOld, claimNew);
            }
            Claims = await _UserManager.GetClaimsAsync(user);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteClaimAsync([Required] string type,
                                                               [Required] string value)
        {
            Accounts user = await _UserManager.FindByIdAsync(Id);
            if (ModelState.IsValid)
            {
                var claim = new Claim(type, value);
                var result = await _UserManager.RemoveClaimAsync(user, claim);
            }
            Claims = await _UserManager.GetClaimsAsync(user);
            return RedirectToPage();

        }

        //public void OnGet()
        //{
        //}
    }
}
