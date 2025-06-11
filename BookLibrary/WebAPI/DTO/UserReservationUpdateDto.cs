using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UserReservationUpdateDto : IUpdateDto
    {
        public int Status { get; set; }

        public DateTime? Date { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }
    }
}
