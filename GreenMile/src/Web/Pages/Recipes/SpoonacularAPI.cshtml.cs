using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Recipes
{
    public class SpoonacularAPIModel : PageModel
    {
        [BindProperty]
        public int numberOfRecipes { get; set; }
        public void OnGet()
        {
            numberOfRecipes = 1;
        }
    }
}
