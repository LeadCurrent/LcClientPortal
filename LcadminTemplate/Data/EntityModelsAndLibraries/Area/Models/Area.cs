using System;
using System.Collections.Generic;

namespace Data;

public partial class Area
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public virtual ICollection<Programarea> Programareas { get; set; } = new List<Programarea>();
}
