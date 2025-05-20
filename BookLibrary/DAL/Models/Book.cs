using System;
using System.Collections.Generic;
using System.Reflection;

namespace DAL.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public int GenreId { get; set; }

    public virtual ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<UserReservation> UserReservations { get; set; } = new List<UserReservation>();

    public virtual ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
}
