using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Models;

namespace Web.Pages
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExternalLoginModel(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            
        }
        public async Task<IActionResult> OnGet()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                TempData["success"] = "Login success!";
  
              
                return Redirect("/");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name).Replace(" ", ""),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = "",
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                          
                            
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        if(identityResult.Succeeded)
                        {

                        } else
                        {
                            Console.WriteLine(identityResult.ToString());
                        }
                        TempData["success"] = "Register success!";
                    }
                    await _userManager.AddLoginAsync(user, info); //adds exteranl login info that links the email together 
                    await _signInManager.SignInAsync(user, isPersistent: true);
    
                }
                TempData["success"] = "Login success!";
                return Redirect("/");
            }

            return Redirect("/login");
        }
    }
}
