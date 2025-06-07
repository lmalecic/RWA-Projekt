using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;

        [HiddenInput]
        public string? ReturnUrl { get; set; } = null!;
    }
}
