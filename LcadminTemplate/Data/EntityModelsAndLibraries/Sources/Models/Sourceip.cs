using System;
using System.Collections.Generic;

namespace Data;

public partial class Sourceip
{
    public int Id { get; set; }

    public int Sourceid { get; set; }

    public string Ipaddress { get; set; }

    public virtual Source Source { get; set; }
}
