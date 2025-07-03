using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserReservationViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int Status { get; set; }

        [Display(Name = "Reservation Date")]
        public DateTime? Date { get; set; }

        [Required]
        [Display(Name = "Book")]
        public BookViewModel Book { get; set; } = null!;
    }
}