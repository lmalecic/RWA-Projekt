using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public int Status { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
