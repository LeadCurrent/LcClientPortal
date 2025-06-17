using System;
using System.Collections.Generic;

namespace Data;

public partial class Leadpost
{
    public int Id { get; set; }

    public string Parameterstring { get; set; }

    public string Serverresponse { get; set; }

    public bool? Testflag { get; set; }

    public string Testparameter { get; set; }

    public bool? Success { get; set; }

    public string Ipaddress { get; set; }

    public DateTime Date { get; set; }

    public string Vamidentifier { get; set; }

    public int? Offerid { get; set; }

    public int? Schoolid { get; set; }

    public int? Sourceid { get; set; }

    public string Zip { get; set; }

    public int? Campusid { get; set; }

    public string Campusname { get; set; }

    public int? Programid { get; set; }

    public string Programname { get; set; }

    public string Clientname { get; set; }

    public string Offername { get; set; }

    public string Agent { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public int? oldId { get; set; }
}
