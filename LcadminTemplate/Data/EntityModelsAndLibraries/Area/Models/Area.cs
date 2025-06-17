using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Area
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<Programarea> Programareas { get; set; } = new List<Programarea>();

    [NotMapped]
    public bool IsChecked { get; set; }
}
