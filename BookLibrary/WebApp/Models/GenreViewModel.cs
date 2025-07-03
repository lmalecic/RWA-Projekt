using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class GenreViewModel
    {
        [Display(Name = "Genre Id")]
        public int Id { get; set; }

        [Display(Name = "Genre name")]
        public string Name { get; set; } = null!;
    }
}
