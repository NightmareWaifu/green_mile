using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;
using Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DonationService _donationService;
        private readonly DonationRequestService _donationRequestService;
        private readonly UserManager<User> _userManager;
        private readonly ChatService _chatService;

        public IndexModel(DonationService donationService, DonationRequestService donationRequestService, UserManager<User> userManager,
            ChatService chatService)
        {
            _donationService = donationService;
            _donationRequestService = donationRequestService;
            _userManager = userManager;
            _chatService = chatService;
        }

        public List<Donation> DonationList { get; set; } = new();

        [BindProperty]
        public int DonationId { get; set; }

        [BindProperty]
        public int Freshproduce { get; set; }
        [BindProperty]
        public int Cannedfood { get; set; }
        [BindProperty]
        public int Snacks { get; set; }
        [BindProperty]
        public int Beverages { get; set; }
        [BindProperty]
        public int Meats { get; set; }

        public async Task OnGetAsync()
        {
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            DonationList = _donationService.GetAll(userId);
            Freshproduce = DonationList.Count(d => d.CustomFood.Category == "Fresh produce");
            Cannedfood = DonationList.Count(d => d.CustomFood.Category == "Canned food");
            Snacks = DonationList.Count(d => d.CustomFood.Category == "Snacks");
            Beverages = DonationList.Count(d => d.CustomFood.Category == "Beverages");
            Meats = DonationList.Count(d => d.CustomFood.Category == "Meats");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Checking for has the recipient requested the item before
            var donation = _donationService.GetDonationById(DonationId);
            var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            User user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            var exists = await _donationRequestService.DonationRequestExists(DonationId, userId);

            // Donor cannot request for his/her own donation
            User donor = donation.User;

            if (donor == user)
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "You are not allowed to request your own donation";
                return Redirect("/FoodSharing/Index");
            }

            if (exists == false)
            {
                var request = new DonationRequest()
                {
                    Donation = donation,
                    Donor = donor,
                    Recipient = user,
                    Status = RequestStatus.PENDING
                };

                await _donationRequestService.AddRequest(request);

                MailMessage message = new MailMessage();
                message.To.Add("liujiajun2003@gmail.com");
                message.Subject = "You have received a request";
                message.Body = $"Hi {donor.UserName},Your received a donation request from user {user.UserName} about the food item {donation.CustomFood.Name}";


                message.IsBodyHtml = false;
                message.From = new MailAddress("greenmile2024bycrazyapi@gmail.com");

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;


                // Green Mile Email Account Password: greenmile2023!
                client.Credentials = new NetworkCredential("greenmile2024bycrazyapi@gmail.com", "arcpfpypntqmhnxf");
                await client.SendMailAsync(message);

                var firstMessage = new MessageHistory()
                {
                    User = user,
                    DonationRequest = request,
                    Text = $"Food Name: {donation.CustomFood.Name} ,,Category: {donation.CustomFood.Category} ,,Pick-Up Location: {donation.Location} ,,Expiry Date: {donation.CustomFood.ExpiryDate.ToString("MM/dd/yyyy")}"
                };

                await _chatService.FirstMessage(firstMessage);

                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "You have request for the items, Please wait for the donor to response";
                return Redirect("/FoodSharing/Index");
            }
            else
            {
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "You have requested for the items before";
                return Redirect("/FoodSharing/Index");

            }



        }
    }
}
