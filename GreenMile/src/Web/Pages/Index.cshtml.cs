using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;

namespace Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly CategoryService _categoryService;

    public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager, SignInManager<User> signInManager, CategoryService categoryService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await _categoryService.Prepopulate();
        try
        {
            return User != null
                && User.Identity.IsAuthenticated
                && !await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Member")
                ? Redirect("/account/transferhousehold")
                : Page();
        }
        catch (Exception)
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Index");
        }
    }
}