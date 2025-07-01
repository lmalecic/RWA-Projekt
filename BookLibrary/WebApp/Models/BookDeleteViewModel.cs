namespace WebApp.Models
{
    public class BookDeleteViewModel
    {
        public BookViewModel Book { get; set; } = null!;
        public string? ReturnUrl { get; set; }
    }
}
