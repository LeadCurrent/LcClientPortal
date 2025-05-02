using System;
using System.Collections.Generic;

namespace Data;

public partial class Allocationcampusdegree
{
    public int Id { get; set; }

    public int Allocationid { get; set; }

    public int Campusdegreeid { get; set; }

    public bool Active { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Wcap { get; set; }

    public int Wcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public virtual Allocation Allocation { get; set; }

    public virtual Campusdegree Campusdegree { get; set; }
}
