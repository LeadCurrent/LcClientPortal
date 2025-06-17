using System;
using System.Collections.Generic;

namespace Data;

public partial class Extrarequirededucation
{
    public int Id { get; set; }

    public int Degreeid { get; set; }

    public string Value { get; set; }

    public int Campusid { get; set; }

    public int? oldId { get; set; }
    public int? CompanyId { get; set; }
    public Company Company { get; set; }
}
