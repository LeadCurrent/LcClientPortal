using System;
using System.Collections.Generic;

namespace Data;

public partial class VwOffer
{
    public int Id { get; set; }
    public string School { get; set; }

    public string Client { get; set; }

    public bool Active { get; set; }

    public decimal Rpl { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public string Type { get; set; }

    public int OfferId { get; set; }
}
