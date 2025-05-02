using System;
using System.Collections.Generic;

namespace Data;

public partial class Interest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Copy { get; set; }

    public virtual ICollection<Programinterest> Programinterests { get; set; } = new List<Programinterest>();
}
