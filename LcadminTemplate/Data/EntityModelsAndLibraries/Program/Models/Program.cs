using System;
using System.Collections.Generic;

namespace Data;

public partial class Program
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();

    public virtual ICollection<Programarea> Programareas { get; set; } = new List<Programarea>();

    public virtual ICollection<Programinterest> Programinterests { get; set; } = new List<Programinterest>();
}
