using DAL.Services;

namespace WebApp.ViewModels
{
    public class BooksViewModel
    {
        public SearchResult<BookViewModel> SearchResult { get; set; } = null!;
        public IEnumerable<GenreViewModel> Genres { get; set; } = null!;
    }
}
