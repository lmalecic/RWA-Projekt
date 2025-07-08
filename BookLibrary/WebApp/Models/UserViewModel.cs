using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(31, ErrorMessage = "Username can't be longer than 31 characters!")]
        public string Username { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        [StringLength(255, ErrorMessage = "Email should be no longer than 255 characters!")]
        public string? Email { get; set; } = null!;

        [Display(Name = "First name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? LastName { get; set; }

        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        [StringLength(50)]
        public string? Phone { get; set; }
    }
}
