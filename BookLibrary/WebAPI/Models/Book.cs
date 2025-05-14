using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Author { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Isbn { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
