using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class UserReservation
{
    public int Id { get; set; }

    public int Status { get; set; }

    public DateTime? Date { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
