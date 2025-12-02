using System;
using System.Collections.Generic;

namespace Labb2_BookStore.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
