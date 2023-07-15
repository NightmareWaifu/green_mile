using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;

using Web.Services;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class ReceivedRequestsModel : PageModel
    {
        private readonly DonationRequestService _donationRequestService;
        private readonly UserManager<User> _userManager;

        public ReceivedRequestsModel(DonationRequestService donationRequestService, UserManager<User> userManager)
        {
            _donationRequestService = donationRequestService;
            _userManager = userManager;
        }

        public List<DonationRequest> DonationRequestList { get; set; } = new();
        public int MyRequestCount { get; set; } = 0;

        public async Task<IActionResult> OnGet()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            DonationRequestList = _donationRequestService.GetRequestByDonor(userId);
            MyRequestCount = DonationRequestList.Count;
            return Page();
        }
        public async Task<IActionResult> OnPostAcceptAsync(int id)
        {
            var donationRequest = await _donationRequestService.GetRequestsByRequestID(id);
            donationRequest.Status = RequestStatus.ACCEPTED;
            donationRequest.Donation.Status = DonationStatus.RESERVED;
            await _donationRequestService.Update(donationRequest);

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = string.Format("You have successfully accepted the request with food {0}", donationRequest.Donation.CustomFood.Name);

            return RedirectToPage("/FoodSharing/ReceivedRequests");
        }
    }
}
