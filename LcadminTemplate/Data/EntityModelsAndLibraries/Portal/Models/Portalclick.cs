using System;
using System.Collections.Generic;

namespace Data;

public partial class Portalclick
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Sourceid { get; set; }

    public string Ipaddress { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Agentemail { get; set; }

    public int? Portalid { get; set; }

    public string Url { get; set; }

    public bool Clicked { get; set; }
}
