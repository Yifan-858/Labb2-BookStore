using System;
using System.Collections.Generic;

namespace Labb2_BookStore.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? StoreAddress { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
