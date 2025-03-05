using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Purchasedetail
{
    public int Detailid { get; set; }

    public int? Purchaseid { get; set; }

    public int? Materialid { get; set; }

    public decimal Quantity { get; set; }

    public decimal Unitprice { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Purchase? Purchase { get; set; }
}
