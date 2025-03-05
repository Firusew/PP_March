using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Warehouse
{
    public int Warehouseid { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public decimal? Capacity { get; set; }

    public virtual ICollection<Materialmovement> Materialmovements { get; set; } = new List<Materialmovement>();

    public virtual ICollection<Warehousestock> Warehousestocks { get; set; } = new List<Warehousestock>();
}
