using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UserReviewDto
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string? Text { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }
    }
}
