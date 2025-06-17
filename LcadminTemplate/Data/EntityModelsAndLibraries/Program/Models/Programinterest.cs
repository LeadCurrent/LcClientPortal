using System;
using System.Collections.Generic;

namespace Data;

public partial class Programinterest
{
    public int Id { get; set; }

    public int Programid { get; set; }

    public int Interestid { get; set; }

    public virtual Interest Interest { get; set; }

    public virtual Program Program { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }


}
