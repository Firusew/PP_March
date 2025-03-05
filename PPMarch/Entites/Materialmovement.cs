using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Materialmovement
{
    public int Movementid { get; set; }

    public int? Materialid { get; set; }

    public int? Warehouseid { get; set; }

    public string Movementtype { get; set; } = null!;

    public decimal Quantity { get; set; }

    public DateTime Movementdate { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
