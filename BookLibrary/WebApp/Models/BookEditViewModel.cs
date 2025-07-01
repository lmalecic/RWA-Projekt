
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BookEditViewModel
    {
        public BookViewModel Book { get; set; } = null!;
        public IEnumerable<GenreViewModel> ExistingGenres = new List<GenreViewModel>();
        public string? ReturnUrl { get; set; }
    }
}
