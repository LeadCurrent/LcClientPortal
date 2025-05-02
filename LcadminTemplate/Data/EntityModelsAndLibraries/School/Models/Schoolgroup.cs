using System;
using System.Collections.Generic;

namespace Data;

public partial class Schoolgroup
{
    public int Id { get; set; }

    public int Groupid { get; set; }

    public int Schoolid { get; set; }

    public virtual Group Group { get; set; }

    public virtual School School { get; set; }
}
