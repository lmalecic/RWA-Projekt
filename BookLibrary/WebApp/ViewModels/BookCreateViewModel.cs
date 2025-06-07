using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class BookCreateViewModel
    {
        [Display(Name = "ISBN")]
        public string Isbn { get; set; } = null!;

        [Display(Name = "Title")]
        public string Name { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        [Display(Name = "Publication Date")]
        public DateOnly? PublicationDate { get; set; }

        [Display(Name = "Genre")]
        public GenreViewModel Genre { get; set; } = null!;
    }
}
