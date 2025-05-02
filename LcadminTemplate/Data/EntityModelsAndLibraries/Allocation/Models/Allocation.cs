using System;
using System.Collections.Generic;

namespace Data;

public partial class Allocation
{
    public int Id { get; set; }

    public int Offerid { get; set; }

    public int Sourceid { get; set; }

    public bool Active { get; set; }

    public string Identifier { get; set; }

    public decimal Cpl { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public bool Wcap { get; set; }

    public int Wcapamt { get; set; }

    public string Transferphone { get; set; }

    public bool CecIncludeA { get; set; }

    public bool CecIncludeB { get; set; }

    public bool CecIncludeC { get; set; }

    public bool CecIncludeD { get; set; }

    public bool CecIncludeE { get; set; }

    public bool CecIncludeF { get; set; }

    public bool CecIncludeG { get; set; }

    public decimal CecCplA { get; set; }

    public decimal CecCplB { get; set; }

    public decimal CecCplC { get; set; }

    public decimal CecCplD { get; set; }

    public decimal CecCplE { get; set; }

    public decimal CecCplF { get; set; }

    public decimal CecCplG { get; set; }

    public virtual ICollection<Allocationcampusdegree> Allocationcampusdegrees { get; set; } = new List<Allocationcampusdegree>();

    public virtual ICollection<Allocationcampus> Allocationcampuses { get; set; } = new List<Allocationcampus>();

    public virtual Offer Offer { get; set; }

    public virtual Source Source { get; set; }
}
