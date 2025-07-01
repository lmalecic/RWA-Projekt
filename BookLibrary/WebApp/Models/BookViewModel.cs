using DAL.Attributes;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BookViewModel
    {
        [HiddenInput]
        [Display(Name = "Book Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; } = null!;

        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; } = string.Empty;

        [Display(Name = "Publication Date")]
        public DateOnly? PublicationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Genre")]
        public GenreViewModel Genre { get; set; } = null!;

        [Required]
        [Display(Name = "Genre")]
        [Exists<Genre>(ErrorMessage = "Genre does not exist!")]
        public int GenreId { get; set; } = 1;

        //public ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();

        //public ICollection<UserReservation> UserReservations { get; set; } = new List<UserReservation>();

        //public ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
    }
}
