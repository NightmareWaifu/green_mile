using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Web.Models;

using Web.Services;

namespace Web.Pages.FoodSharing
{
    [Authorize]
    public class EditDonationInfoModel : PageModel
    {
        private readonly CustomFoodService _customFoodService;
        private readonly DonationService _donationService;
        private readonly IWebHostEnvironment _environment;

        public EditDonationInfoModel(
            CustomFoodService customFoodService,
            DonationService donationService,
            IWebHostEnvironment environment
        )
        {
            _donationService = donationService;
            _customFoodService = customFoodService;
            _environment = environment;
        }

        [BindProperty]
        public int CustomFoodId { get; set; }

        [BindProperty] 
        public int DonationId { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public string? Category { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty, Required, DataType(DataType.Date), Display(Name = "Expiry Date")]

        public DateTime? ExpiryDate { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public string Image { get; set; }

        [BindProperty]
        public string Status { get; set; }

        [BindProperty]
        public string Location { get; set; }

        public IActionResult OnGet(int id)
        {
            var donation = _donationService.GetDonationById(id);

            if (donation is null)
                return NotFound();

            CustomFoodId = donation.CustomFood.Id;
            DonationId = donation.Id;
            Name = donation.CustomFood.Name;
            Category = donation.CustomFood.Category;
            Description= donation.CustomFood.Description;
            ExpiryDate = donation.CustomFood.ExpiryDate;
            Image = donation.CustomFood.Image;
            Status = donation.Status.ToString();
            Location = donation.Location;
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var customFood = await _customFoodService.GetCustomFoodById(CustomFoodId);
            var donation  = _donationService.GetDonationById(DonationId);

            if (Status == DonationStatus.ACTIVE.ToString())
            {
                donation!.Status = DonationStatus.ACTIVE;
            }
            else if (Status == DonationStatus.RESERVED.ToString())
            {
                donation!.Status = DonationStatus.RESERVED;
            }
            else if (Status == DonationStatus.COMPLETED.ToString())
            {
                donation!.Status = DonationStatus.COMPLETED;
            }
            else if (Status == DonationStatus.CANCELLED.ToString())
            {
                donation!.Status = DonationStatus.CANCELLED;
            }

            customFood!.Name = Name ?? customFood.Name;
            customFood!.Category = Category ?? customFood.Category;
            customFood!.Description = Description ?? customFood.Description;
            customFood!.ExpiryDate = ExpiryDate ?? customFood.ExpiryDate;
            
            donation!.Location = Location ?? donation.Location;

            if (Upload is not null)
            {
                var uploadFolder = "Uploads";
                var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadFolder, imageFile);
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                await Upload.CopyToAsync(fileStream);
                customFood.Image = string.Format("/{0}/{1}", uploadFolder, imageFile);
            }

            await _donationService.UpdateDonation(donation);
            await _customFoodService.Update(customFood);

            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "You have successfully updated the food item";
            return Redirect("/FoodSharing/Mydonations");
        }
    }
}
