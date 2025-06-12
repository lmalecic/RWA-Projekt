using DAL.Attributes;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DTO
{
    public class BookDto
    {
        public int Id { get; set; }

        public string Isbn { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly? PublicationDate { get; set; }

        [Exists<Genre>]
        public int GenreId { get; set; }
    }
}
