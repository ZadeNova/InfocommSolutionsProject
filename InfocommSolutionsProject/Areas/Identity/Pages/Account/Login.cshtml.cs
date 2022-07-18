// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InfocommSolutionsProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using InfocommSolutionsProject.Models;
using System.Security.Claims;
using AspNetCore.ReCaptcha;
using InfocommSolutionsProject.Data;
using Microsoft.AspNetCore.Authentication.Google;

namespace InfocommSolutionsProject.Areas.Identity.Pages.Account
{

    [ValidateReCaptcha]
   
    public class LoginModel : PageModel
    {
     
        private readonly SignInManager<Accounts> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<Accounts> _userManager;
        public LoginModel(SignInManager<Accounts> signInManager, ILogger<LoginModel> logger, UserManager<Accounts> userManager)
        {
            _signInManager = signInManager;
            
            _logger = logger;
            _userManager = userManager;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
              
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var accessToken = await HttpContext.GetTokenAsync(
            GoogleDefaults.AuthenticationScheme, "access_token");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        //public IActionResult Externallogin(string provider, string returnUrl) {
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
        //               new { ReturnUrl = returnUrl });
        //    var properties = _signInManager
        //        .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties);
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
           
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //Claim aclaim = new Claim("LoggedIn", Input.Email);
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //System.Diagnostics.Debug.WriteLine($"Is it true:{User.IsInRole("ADMIN")}");
                    //System.Diagnostics.Debug.WriteLine($"Is it true:{User.IsInRole("Customer")}");
                    //System.Diagnostics.Debug.WriteLine($"Is it true:{User.IsInRole("SGAE_Staff")}");
                    //System.Diagnostics.Debug.WriteLine($"Is it true:{User.IsInRole(ApplicationRoles.AdminRole)}");
                    //System.Diagnostics.Debug.WriteLine($"Is it true:{User.IsInRole("Admin")}");
                    
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user.AccountStatus == "Activate")
                    {
                        System.Diagnostics.Debug.WriteLine($"{user.FirstName} {user.Email} {user.LastName}");
                        var roles = await _userManager.GetRolesAsync(user);
                        //foreach (var i in roles) System.Diagnostics.Debug.WriteLine(i);

                        // check for roles
                        if (roles.Contains("Admin"))
                        {
                            System.Diagnostics.Debug.WriteLine("Hi there admin!");
                            _logger.LogInformation("Admin logged in.");
                            return Redirect("~/AdminHomePage");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Hello there User");
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else if (user.AccountStatus == "Deactivated") {
                        ModelState.AddModelError(String.Empty, "Your Account has been Deactivated , kindly contact the customer support ! thank you ");
                        await _signInManager.SignOutAsync();
                        return Page();
                    }
                    else {
                        ModelState.AddModelError(String.Empty, "Your Account has been Ban , kindly contact the customer support ! thank you ");
                        await _signInManager.SignOutAsync();
                        return Page();
                    }
                   
                    


                    //if (User.IsInRole("Admin"))
                    //{
                    //    System.Diagnostics.Debug.WriteLine("Hi there admin!");
                    //    _logger.LogInformation("Admin logged in.");
                    //    return Redirect("~/AdminHomePage");
                    //}
                    //else {
                    //    System.Diagnostics.Debug.WriteLine("Hello there User");
                    //    _logger.LogInformation("User logged in.");
                    //    return LocalRedirect(returnUrl);
                    //}

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(String.Empty, "Click the recaptcha to submit ");
            return Page();
        }
    }
}
