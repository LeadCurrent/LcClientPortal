using System;
using System.Collections.Generic;

namespace Data;

public partial class VwSchoolGroup
{
    public int Id { get; set; }
    public int Groupid { get; set; }

    public string Groupname { get; set; }

    public string Groupcopy { get; set; }

    public int Schoolid { get; set; }

    public string Schoolname { get; set; }
}
