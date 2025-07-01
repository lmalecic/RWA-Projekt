using DAL.Attributes;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BookCreateViewModel
    {
        public BookViewModel Book { get; set; } = new();
        public IEnumerable<GenreViewModel> ExistingGenres = new List<GenreViewModel>();
        public string? ReturnUrl { get; set; }
    }
}
