using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class BookLog : IComparable
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public int Level { get; set; }

    public string Message { get; set; } = null!;

    public BookLog(string message, int level)
    {
        this.Timestamp = DateTime.Now;
        this.Message = message;
        this.Level = level;
    }

    public int CompareTo(object? obj)
    {
        return obj is BookLog other ? -Timestamp.CompareTo(other.Timestamp) : 0;
    }
}
