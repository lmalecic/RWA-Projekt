using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class BookSearchParams
    {
        [FromQuery(Name = "count")]
        [DefaultValue(10)]
        [Required]
        public int Count { get; set; } = 10;

        [FromQuery(Name = "page")]
        [DefaultValue(1)]
        [Required]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "name")]
        public string? Name { get; set; } = null;

        [FromQuery(Name = "author")]
        public string? Author { get; set; } = null;

        [FromQuery(Name = "description")]
        public string? Description { get; set; } = null;

        [FromQuery(Name = "genreId")]
        public int? GenreId { get; set; } = null;
    }
}
