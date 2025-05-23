﻿using System.ComponentModel.DataAnnotations;

namespace DAL.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string? FirstName { get; set; }

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string? Phone { get; set; }
    }
}