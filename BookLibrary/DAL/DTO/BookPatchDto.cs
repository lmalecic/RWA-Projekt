using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class BookPatchDto
    {
        public string? Isbn { get; set; } = null!;

        public string? Name { get; set; } = null!;

        public string? Author { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly? PublicationDate { get; set; }

        public int? GenreId { get; set; }
    }
}
