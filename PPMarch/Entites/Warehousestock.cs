using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Warehousestock
{
    public int Stockid { get; set; }

    public int? Warehouseid { get; set; }

    public int? Materialid { get; set; }

    public decimal Quantity { get; set; }

    public DateTime Lastupdated { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
