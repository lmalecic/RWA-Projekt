
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class BookEditViewModel
    {
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
        public GenreViewModel Genre { get; set; } = null!;

        //[Display(Name = "Locations")]
        //public ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();
    }
}
