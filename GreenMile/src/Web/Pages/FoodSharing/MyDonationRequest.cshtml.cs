using System.Xml.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Web.Models;
using Web.Services;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class MyDonationRequestModel : PageModel
    {
        private readonly DonationRequestService _donationRequestService;
        private readonly UserManager<User> _userManager;

        public MyDonationRequestModel(DonationRequestService donationRequestService, UserManager<User> userManager)
        {
            _donationRequestService = donationRequestService;
            _userManager = userManager;
        }
        public List<DonationRequest> DonationRequestList { get; set; } = new();
        public int MyRequestCount { get; set; } = 0;

        public async Task<IActionResult> OnGet()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            DonationRequestList = _donationRequestService.GetRequestsByRecipient(userId);
            MyRequestCount = DonationRequestList.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var donationRequest = await _donationRequestService.GetRequestsByRequestID(id);
            donationRequest.Status = RequestStatus.CANCELLED;
            await _donationRequestService.Update(donationRequest);

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = string.Format("You have successfully cancelled the request with food item {0}", donationRequest.Donation.CustomFood.Name);

            return RedirectToPage("/FoodSharing/MyDonationRequest");
        }
    }
}
