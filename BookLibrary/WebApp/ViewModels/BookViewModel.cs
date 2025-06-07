using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class BookViewModel
    {
        [Display(Name = "Book Id")]
        public int Id { get; set; }

        [Display(Name = "ISBN")]
        public string Isbn { get; set; } = null!;

        [Display(Name = "Title")]
        public string Name { get; set; } = null!;

        [Display(Name = "Author")]
        public string Author { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; } = "";

        [Display(Name = "Publication Date")]
        public DateOnly? PublicationDate { get; set; }

        [Display(Name = "Genre")]
        public virtual Genre Genre { get; set; } = null!;

        public virtual ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();

        public virtual ICollection<UserReservation> UserReservations { get; set; } = new List<UserReservation>();

        public virtual ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
    }
}
