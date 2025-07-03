using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ReservationCreateViewModel
    {
        [ValidateNever]
        public UserViewModel User { get; set; } = null!;
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Reservation Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ValidateNever]
        [Display(Name = "Available Books")]
        public List<BookViewModel> AvailableBooks { get; set; } = new();
    }
}