using System;
using System.Collections.Generic;

namespace Data;

public partial class Campuspostalcode
{
    public int Id { get; set; }

    public int Campusid { get; set; }

    public int Postalcodeid { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }

    public virtual Campus Campus { get; set; }

    public virtual Postalcode Postalcode { get; set; }
}
