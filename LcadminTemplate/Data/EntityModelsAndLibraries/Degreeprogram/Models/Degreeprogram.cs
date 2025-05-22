using System;
using System.Collections.Generic;

namespace Data;

public partial class Degreeprogram
{
    public int Id { get; set; }

    public int Levelid { get; set; }

    public int Programid { get; set; }

    public string Copy { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<Campusdegree> Campusdegrees { get; set; } = new List<Campusdegree>();

    public virtual Level Level { get; set; }

    public virtual Program Program { get; set; }
}
