using System;
using System.Collections.Generic;

namespace Labb2_BookStore.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public int? CustomerId { get; set; }

    public string? Isbn13 { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Book? Isbn13Navigation { get; set; }
}
