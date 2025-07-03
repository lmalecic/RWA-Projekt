using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ReviewCreateViewModel
    {
        public int UserId { get; set; }
        [ValidateNever]
        public UserViewModel User { get; set; } = null!;

        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;

        [ValidateNever]
        [Display(Name = "Available Books")]
        public List<BookViewModel> AvailableBooks { get; set; } = new();
    }
}
