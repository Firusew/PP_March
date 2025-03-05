using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Purchase
{
    public int Purchaseid { get; set; }

    public DateOnly Purchasedate { get; set; }

    public int? Supplierid { get; set; }

    public string Status { get; set; } = null!;

    public decimal Totalamount { get; set; }

    public virtual ICollection<Purchasedetail> Purchasedetails { get; set; } = new List<Purchasedetail>();

    public virtual Supplier? Supplier { get; set; }
}
