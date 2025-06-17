using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Interest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public virtual ICollection<Programinterest> Programinterests { get; set; } = new List<Programinterest>();

    [NotMapped]
    public bool IsChecked { get; set; }
    public int? oldId { get; set; }

    public int? CompanyId { get; set; }
    public Company Company { get; set; }
    
}
