using System;
using System.Collections.Generic;

namespace Labb2_BookStore.Models;

public partial class CustomerOrder
{
    public int OrderId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public decimal? Amount { get; set; }

    public int? CustomerId { get; set; }

    public int? StoreId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Store? Store { get; set; }
}
