using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DTO
{
    public class BookLocationDto
    {
        [FromQuery(Name = "bookId")]
        [Required]
        public int BookId { get; set; }

        [FromQuery(Name = "locationId")]
        [Required]
        public int LocationId { get; set; }
    }
}
