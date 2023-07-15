using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.FoodSharing
{
    public class DonationChatModel : PageModel
    {
        [BindProperty]
        public int requestId { get; set; }
        public void OnGet(int id)
        {
            requestId = id;
        }
    }
}
