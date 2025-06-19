using System;
using System.Collections.Generic;

namespace Data;

public partial class MasterSchoolMapping
{
    public int Id { get; set; }

    public int MasterSchoolsId { get; set; }

    public string Identifier { get; set; }
    public int? oldId { get; set; }

    public virtual MasterSchool MasterSchools { get; set; }
}
