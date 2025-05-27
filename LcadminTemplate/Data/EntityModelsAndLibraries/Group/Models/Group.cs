using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public virtual ICollection<Schoolgroup> Schoolgroups { get; set; } = new List<Schoolgroup>();
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }

    [NotMapped]
    public bool IsChecked { get; set; }
}
