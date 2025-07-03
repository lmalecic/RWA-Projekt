using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Location name")]
        public string Name { get; set; } = null!;
    }
}
