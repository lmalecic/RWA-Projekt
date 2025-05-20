using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookLocation> BookLocations { get; set; } = new List<BookLocation>();
}
