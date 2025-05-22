using System;
using System.Collections.Generic;

namespace Data;

public partial class MasterSchool
{
    public int Id { get; set; }

    public string Name { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    public virtual ICollection<MasterSchoolMapping> MasterSchoolMappings { get; set; } = new List<MasterSchoolMapping>();
}
