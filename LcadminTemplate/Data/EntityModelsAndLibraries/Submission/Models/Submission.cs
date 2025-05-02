using System;
using System.Collections.Generic;

namespace Data;

public partial class Submission
{
    public int Id { get; set; }

    public int Offerid { get; set; }

    public int Sourceid { get; set; }

    public DateTime Date { get; set; }

    public string Ipaddress { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Postalcode { get; set; }

    public string Userconfidencelevel { get; set; }

    public string Agentconfidencelevel { get; set; }

    public string Partnerid { get; set; }
}
