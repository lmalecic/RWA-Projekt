using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BookLog
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int Level { get; set; }

    public string Message { get; set; } = null!;
}
