using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ReservationEditViewModel
    {
        public int Id { get; set; }

        [ValidateNever]
        public UserViewModel User { get; set; } = null!;
        public int UserId { get; set; }

        [ValidateNever]
        public BookViewModel Book { get; set; } = null!;
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int Status { get; set; }

        [Required]
        [Display(Name = "Reservation Date")]
        public DateTime Date { get; set; }
    }
}