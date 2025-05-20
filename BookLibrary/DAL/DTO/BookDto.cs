using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class BookDto
    {
        public int Id { get; set; }

        public string Isbn { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly? PublicationDate { get; set; }

        public int GenreId { get; set; }

        public BookDto Clone() => new BookDto
        {
            Id = Id,
            Isbn = Isbn,
            Name = Name,
            Author = Author,
            Description = Description,
            PublicationDate = PublicationDate,
            GenreId = GenreId
        };
    }
}
