using System;
using System.Collections.Generic;

namespace Data;

public partial class Level
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }
    public int? oldId { get; set; }  

    public int? CompanyId { get; set; }
    public Company Company { get; set; }

    public virtual ICollection<Degreeprogram> Degreeprograms { get; set; } = new List<Degreeprogram>();
}
