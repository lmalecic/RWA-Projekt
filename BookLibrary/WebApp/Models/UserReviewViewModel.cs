using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserReviewViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Display(Name = "Comment")]
        public string? Text { get; set; }

        [Required]
        [Display(Name = "Book")]
        public BookViewModel Book { get; set; } = null!;
    }
}