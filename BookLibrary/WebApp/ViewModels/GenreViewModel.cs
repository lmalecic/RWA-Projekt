using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class GenreViewModel
    {
        [Display(Name = "Genre Id")]
        public int Id { get; set; }

        [Display(Name = "Genre name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Books related to this genre")]
        public ICollection<BookViewModel> Books { get; set; } = new List<BookViewModel>();
    }
}
