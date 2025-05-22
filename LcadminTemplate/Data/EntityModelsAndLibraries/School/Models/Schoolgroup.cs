using System;
using System.Collections.Generic;

namespace Data;

public partial class Schoolgroup
{
    public int Id { get; set; }

    public int Groupid { get; set; }

    public int Schoolid { get; set; }

    public virtual Group Group { get; set; }

    public virtual Scholls School { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
}
