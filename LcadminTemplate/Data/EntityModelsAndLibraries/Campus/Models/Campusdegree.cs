using System;
using System.Collections.Generic;

namespace Data;

public partial class Campusdegree
{
    public int Id { get; set; }

    public int Campusid { get; set; }

    public int Degreeid { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public bool Active { get; set; }

    public string Clientid { get; set; }

    public virtual ICollection<Allocationcampusdegree> Allocationcampusdegrees { get; set; } = new List<Allocationcampusdegree>();

    public virtual Degreeprogram Degree { get; set; }
}
