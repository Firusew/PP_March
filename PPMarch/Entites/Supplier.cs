using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Supplier
{
    public int Supplierid { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Contactperson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
