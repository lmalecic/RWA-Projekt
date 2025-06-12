namespace WebApp.ViewModels
{
    public class BookCreateViewModel
    {
        public string Isbn { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly? PublicationDate { get; set; }

        public GenreViewModel Genre = null!;
    }
}
