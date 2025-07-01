using DAL.Services;

namespace WebApp.Models
{
    public class BooksViewModel
    {
        public SearchResult<BookViewModel> SearchResult { get; set; } = null!;
        public IEnumerable<GenreViewModel> Genres { get; set; } = null!;
    }
}
