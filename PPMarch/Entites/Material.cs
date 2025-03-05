using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Material
{
    public int Materialid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public decimal? Minstock { get; set; }

    public virtual ICollection<Materialmovement> Materialmovements { get; set; } = new List<Materialmovement>();

    public virtual ICollection<Materialquality> Materialqualities { get; set; } = new List<Materialquality>();

    public virtual ICollection<Purchasedetail> Purchasedetails { get; set; } = new List<Purchasedetail>();

    public virtual ICollection<Warehousestock> Warehousestocks { get; set; } = new List<Warehousestock>();
}
