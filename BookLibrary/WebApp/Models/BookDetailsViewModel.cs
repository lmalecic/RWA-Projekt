namespace WebApp.Models
{
    public class BookDetailsViewModel
    {
        public BookViewModel Book { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
