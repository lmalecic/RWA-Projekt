using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ReviewEditViewModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public UserViewModel User { get; set; } = null!;
        public int UserId { get; set; }
        [ValidateNever]
        public BookViewModel Book { get; set; } = null!;
        public int BookId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;
    }
}