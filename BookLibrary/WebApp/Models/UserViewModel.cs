using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string Email { get; set; } = null!;

        [Display(Name = "First name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string? Phone { get; set; }

        [HiddenInput]
        public string? ReturnUrl { get; set; }
    }
}
