using System;
using System.Collections.Generic;

namespace Data;

public partial class Programarea
{
    public int Id { get; set; }

    public int Programid { get; set; }

    public int Areaid { get; set; }

    public virtual Area Area { get; set; }

    public virtual Program Program { get; set; }
}
