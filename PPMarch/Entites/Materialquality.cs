using System;
using System.Collections.Generic;

namespace PPMarch.Entites;

public partial class Materialquality
{
    public int Qualitycheckid { get; set; }

    public int? Materialid { get; set; }

    public DateOnly Checkdate { get; set; }

    public string Result { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Material? Material { get; set; }
}
