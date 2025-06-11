namespace WebApp.ViewModels
{
    public class BooksViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; }
    }
}
