using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class BookLocationDto : IAssociationDto
    {
        public int BookId { get; set; }

        public int LocationId { get; set; }
    }
}
