using System;
using System.Collections.Generic;

namespace Data;

public partial class Eduapi
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Clientid { get; set; }

    public bool Active { get; set; }

    public string Type { get; set; }

    public string Url { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Wcap { get; set; }

    public int Wcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public virtual Client Client { get; set; }

    public virtual ICollection<Eduapitargeting> Eduapitargetings { get; set; } = new List<Eduapitargeting>();
}
