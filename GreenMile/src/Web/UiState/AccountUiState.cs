using System.ComponentModel.DataAnnotations;

using Web.Models;
namespace Web.UiState
{
    public class AccountUiState
    {
        public string? Tab { get; set; } = "dashboard";
        [Required]
        public string? Username { get; set; } = null;
        [Required]
        public string? FirstName { get;set; } = null;
        [Required]
        public string? LastName { get; set; } = null;
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null;
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; } = null;
        [DataType(DataType.Password), Compare(nameof(ConfirmPassword), ErrorMessage ="New password is not equal to confirm password!")]
        public string? NewPassword { get; set; } = null;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; } = null;
        public Household? Household { get; set; } = null;
        public string? DeletePassword { get; set; } = null;
        public string? DeleteUsername { get; set; } = null;

        [Microsoft.AspNetCore.Mvc.BindProperty]
        public IFormFile? Upload { get; set; }
        public bool? GeneratedImage { get; set; }
      
        public bool HasImageChanged { get; set; } = false;
        public string? GeneratedImageUrl { get; set; } = null;


    }
}
